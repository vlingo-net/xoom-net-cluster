// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Application;
using Vlingo.Cluster.Model.Attribute.Message;
using Vlingo.Cluster.Model.Outbound;
using Vlingo.Common;
using Vlingo.Wire.Message;

namespace Vlingo.Cluster.Model.Attribute
{
    using Vlingo.Wire.Node;
    
    public sealed class AttributesAgentActor : Actor, IAttributesAgent
    {
        private readonly AttributesClient _client;
        private readonly IConfiguration _configuration;
        private readonly IConfirmationInterest _confirmationInterest;
        private readonly ConfirmingDistributor _confirmingDistributor;
        private readonly Node _node;
        private readonly RemoteAttributeRequestHandler _remoteRequestHandler;
        private readonly AttributeSetRepository _repository;

        public AttributesAgentActor(
            Node node,
            IClusterApplication application,
            IOperationalOutboundStream outbound,
            IConfiguration configuration) : this(node, application, outbound, configuration, new NoOpConfirmationInterest(configuration))
        {
        }
        
        public AttributesAgentActor(
            Node node,
            IClusterApplication application,
            IOperationalOutboundStream outbound,
            IConfiguration configuration,
            IConfirmationInterest confirmationInterest)
        {
            _node = node;
            _configuration = configuration;
            _confirmationInterest = confirmationInterest;
            _client = AttributesClient.With(SelfAs<IAttributesAgent>());
            _confirmingDistributor = new ConfirmingDistributor(application, node, outbound, configuration);
            _repository = new AttributeSetRepository();
            _remoteRequestHandler = new RemoteAttributeRequestHandler(_confirmingDistributor, configuration, _repository);
            
            application.InformAttributesClient(_client);

            Stage.Scheduler.Schedule(SelfAs<IScheduled<object?>>(), null, TimeSpan.FromMilliseconds(1000L), 
                TimeSpan.FromMilliseconds(Properties.Instance.ClusterAttributesRedistributionInterval()));
        }
        
        //=========================================
        // AttributesAgent (core)
        //=========================================
        #region AttributesAgent (core)
        
        public void Add<T>(string attributeSetName, string attributeName, T value)
        {
            var set = _repository.AttributeSetOf(attributeSetName);
            
            if (set.IsNone)
            {
                var newSet = AttributeSet.Named(attributeSetName);
                newSet.AddIfAbsent(Attribute<T>.From(attributeName, value));
                _repository.Add(newSet);
                _client.SyncWith(newSet);
                _confirmingDistributor.DistributeCreate(newSet);
            }
            else
            {
                var newlyTracked = set.AddIfAbsent(Attribute<T>.From(attributeName, value));
                if (!newlyTracked.IsDistributed)
                {
                    _confirmingDistributor.Distribute(set, newlyTracked, ApplicationMessageType.AddAttribute);
                }
            }
        }

        public void Replace<T>(string attributeSetName, string attributeName, T value)
        {
            var set = _repository.AttributeSetOf(attributeSetName);
    
            if (!set.IsNone)
            {
                var tracked = set.AttributeNamed(attributeName);
      
                if (tracked.IsPresent)
                {
                    var other = Attribute<T>.From(attributeName, value);
        
                    if (!tracked.SameAs(other))
                    {
                        var newlyTracked = set.Replace(tracked.ReplacingValueWith(other));
          
                        if (newlyTracked.IsPresent)
                        {
                            _client.SyncWith(set);
                            _confirmingDistributor.Distribute(set, newlyTracked, ApplicationMessageType.ReplaceAttribute);
                        }
                    }
                }
            }
        }

        public void Remove(string attributeSetName, string attributeName)
        {
            var set = _repository.AttributeSetOf(attributeSetName);
    
            if (!set.IsNone)
            {
                var tracked = set.AttributeNamed(attributeName);
      
                if (tracked.IsPresent)
                {
                    var untracked = set.Remove(tracked.Attribute);
        
                    if (untracked.IsPresent)
                    {
                        _client.SyncWith(set);
                        _confirmingDistributor.Distribute(set, untracked, ApplicationMessageType.RemoveAttribute);
                    }
                }
            }    
        }

        public void RemoveAll(string attributeSetName)
        {
            var set = _repository.AttributeSetOf(attributeSetName);
    
            if (!set.IsNone)
            {
                _repository.Remove(attributeSetName);
                _client.SyncWithout(set);
                _confirmingDistributor.DistributeRemove(set);
            }    
        }
        
        #endregion

        //=========================================
        // NodeSynchronizer
        //=========================================
        #region NodeSynchronizer
        
        public void Synchronize(Node nodeToSynchronize)
        {
            if (!_node.Equals(nodeToSynchronize))
            {
                _confirmingDistributor.SynchronizeTo(_repository.All, nodeToSynchronize);
            }
        }
        
        #endregion

        //=========================================
        // InboundStreamInterest (operations App)
        //=========================================

        #region InboundStreamInterest (operations App)

        public void HandleInboundStreamMessage(AddressType addressType, RawMessage message)
        {
            if (addressType.IsOperational)
            {
                var request = new ReceivedAttributeMessage(message);
                var type = request.Type;
      
                switch (type)
                {
                    case ApplicationMessageType.CreateAttributeSet:
                        _remoteRequestHandler.CreateAttributeSet(request);
                        break;
                    case ApplicationMessageType.AddAttribute:
                        _remoteRequestHandler.AddAttribute(request);
                        break;
                    case ApplicationMessageType.ReplaceAttribute:
                        _remoteRequestHandler.ReplaceAttribute(request);
                        break;
                    case ApplicationMessageType.RemoveAttribute:
                        _remoteRequestHandler.RemoveAttribute(request);
                        break;
                    case ApplicationMessageType.RemoveAttributeSet:
                        _remoteRequestHandler.RemoveAttributeSet(request);
                        break;
                    case ApplicationMessageType.ConfirmCreateAttributeSet:
                    case ApplicationMessageType.ConfirmAddAttribute:
                    case ApplicationMessageType.ConfirmReplaceAttribute:
                    case ApplicationMessageType.ConfirmRemoveAttribute:
                    case ApplicationMessageType.ConfirmRemoveAttributeSet:
                        _confirmingDistributor.AcknowledgeConfirmation(request.CorrelatingMessageId, _configuration.NodeMatching(request.SourceNodeId));
                        _confirmationInterest.Confirm(request.SourceNodeId, request.AttributeSetName, request.AttributeName, type);
                        break;
                    default:
                        _configuration.Logger.Warn($"Received unknown message: {type.ToString()}");
                        break;
                }
            }
        }

        #endregion

        //=========================================
        // Scheduled
        //=========================================
        
        #region Scheduled
        
        public void IntervalSignal(IScheduled<object> scheduled, object data) => _confirmingDistributor.RedistributeUnconfirmed();
        
        #endregion

        //=========================================
        // Stoppable
        //=========================================
        
        #region Stoppable
        
        public override void Stop()
        {
            if (IsStopped)
            {
                return;
            }
    
            _repository.RemoveAll();
    
            base.Stop();
        }
        
        #endregion
    }
}