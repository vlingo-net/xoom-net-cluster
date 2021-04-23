// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Cluster.Model.Attribute;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Vlingo.Xoom.Actors;

namespace Vlingo.Cluster.Model.Application
{
    using Vlingo.Wire.Node;
    
    public class ClusterApplication__Proxy : IClusterApplication
    {
        private const string HandleApplicationMessageRepresentation1 =
            "HandleApplicationMessage(RawMessage, IApplicationOutboundStream)";

        private const string InformAllLiveNodesRepresentation2 = "InformAllLiveNodes(IEnumerable<Node>, bool)";
        private const string InformLeaderElectedRepresentation3 = "InformLeaderElected(Id, bool, bool)";
        private const string InformLeaderLostRepresentation4 = "InformLeaderLost(Id, bool)";
        private const string InformLocalNodeShutDownRepresentation5 = "InformLocalNodeShutDown(Id)";
        private const string InformLocalNodeStartedRepresentation6 = "InformLocalNodeStarted(Id)";
        private const string InformNodeIsHealthyRepresentation7 = "InformNodeIsHealthy(Id, bool)";
        private const string InformNodeJoinedClusterRepresentation8 = "InformNodeJoinedCluster(Id, bool)";
        private const string InformNodeLeftClusterRepresentation9 = "InformNodeLeftCluster(Id, bool)";
        private const string InformQuorumAchievedRepresentation10 = "InformQuorumAchieved()";
        private const string InformQuorumLostRepresentation11 = "InformQuorumLost()";
        private const string InformResponderRepresentation12 = "InformResponder(IApplicationOutboundStream)";
        private const string InformAttributesClientRepresentation13 = "InformAttributesClient(IAttributesProtocol)";
        private const string InformAttributeSetCreatedRepresentation14 = "InformAttributeSetCreated(string)";
        private const string InformAttributeAddedRepresentation15 = "InformAttributeAdded(string, string)";
        private const string InformAttributeRemovedRepresentation16 = "InformAttributeRemoved(string, string)";
        private const string InformAttributeSetRemovedRepresentation17 = "InformAttributeSetRemoved(string)";
        private const string InformAttributeReplacedRepresentation18 = "InformAttributeReplaced(string, string)";
        private const string StartRepresentation19 = "Start()";
        private const string ConcludeRepresentation20 = "Conclude()";
        private const string StopRepresentation21 = "Stop()";

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public ClusterApplication__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public bool IsStopped => false;

        public void HandleApplicationMessage(RawMessage message)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.HandleApplicationMessage(message);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, HandleApplicationMessageRepresentation1);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        HandleApplicationMessageRepresentation1));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor,
                    HandleApplicationMessageRepresentation1));
            }
        }

        public void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAllLiveNodes(liveNodes, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAllLiveNodesRepresentation2);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAllLiveNodesRepresentation2));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformAllLiveNodesRepresentation2));
            }
        }

        public void InformLeaderElected(Id leaderId, bool isHealthyCluster, bool isLocalNodeLeading)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ =>
                    __.InformLeaderElected(leaderId, isHealthyCluster, isLocalNodeLeading);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformLeaderElectedRepresentation3);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformLeaderElectedRepresentation3));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformLeaderElectedRepresentation3));
            }
        }

        public void InformLeaderLost(Id lostLeaderId, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformLeaderLost(lostLeaderId, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformLeaderLostRepresentation4);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformLeaderLostRepresentation4));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformLeaderLostRepresentation4));
            }
        }

        public void InformLocalNodeShutDown(Id nodeId)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformLocalNodeShutDown(nodeId);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformLocalNodeShutDownRepresentation5);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformLocalNodeShutDownRepresentation5));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(
                    new DeadLetter(_actor, InformLocalNodeShutDownRepresentation5));
            }
        }

        public void InformLocalNodeStarted(Id nodeId)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformLocalNodeStarted(nodeId);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformLocalNodeStartedRepresentation6);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformLocalNodeStartedRepresentation6));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor,
                    InformLocalNodeStartedRepresentation6));
            }
        }

        public void InformNodeIsHealthy(Id nodeId, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformNodeIsHealthy(nodeId, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformNodeIsHealthyRepresentation7);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformNodeIsHealthyRepresentation7));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformNodeIsHealthyRepresentation7));
            }
        }

        public void InformNodeJoinedCluster(Id nodeId, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformNodeJoinedCluster(nodeId, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformNodeJoinedClusterRepresentation8);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformNodeJoinedClusterRepresentation8));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(
                    new DeadLetter(_actor, InformNodeJoinedClusterRepresentation8));
            }
        }

        public void InformNodeLeftCluster(Id nodeId, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformNodeLeftCluster(nodeId, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformNodeLeftClusterRepresentation9);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformNodeLeftClusterRepresentation9));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformNodeLeftClusterRepresentation9));
            }
        }

        public void InformQuorumAchieved()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformQuorumAchieved();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformQuorumAchievedRepresentation10);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformQuorumAchievedRepresentation10));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformQuorumAchievedRepresentation10));
            }
        }

        public void InformQuorumLost()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformQuorumLost();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformQuorumLostRepresentation11);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformQuorumLostRepresentation11));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformQuorumLostRepresentation11));
            }
        }

        public void InformResponder(IApplicationOutboundStream? responder)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformResponder(responder);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformResponderRepresentation12);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformResponderRepresentation12));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformResponderRepresentation12));
            }
        }

        public void InformAttributesClient(IAttributesProtocol client)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributesClient(client);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributesClientRepresentation13);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributesClientRepresentation13));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(
                    new DeadLetter(_actor, InformAttributesClientRepresentation13));
            }
        }

        public void InformAttributeSetCreated(string? attributeSetName)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeSetCreated(attributeSetName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributeSetCreatedRepresentation14);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeSetCreatedRepresentation14));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor,
                    InformAttributeSetCreatedRepresentation14));
            }
        }

        public void InformAttributeAdded(string attributeSetName, string? attributeName)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeAdded(attributeSetName, attributeName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributeAddedRepresentation15);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeAddedRepresentation15));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformAttributeAddedRepresentation15));
            }
        }

        public void InformAttributeRemoved(string attributeSetName, string? attributeName)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeRemoved(attributeSetName, attributeName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributeRemovedRepresentation16);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeRemovedRepresentation16));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(
                    new DeadLetter(_actor, InformAttributeRemovedRepresentation16));
            }
        }

        public void InformAttributeSetRemoved(string? attributeSetName)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeSetRemoved(attributeSetName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributeSetRemovedRepresentation17);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeSetRemovedRepresentation17));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor,
                    InformAttributeSetRemovedRepresentation17));
            }
        }

        public void InformAttributeReplaced(string attributeSetName, string? attributeName)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ =>
                    __.InformAttributeReplaced(attributeSetName, attributeName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributeReplacedRepresentation18);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeReplacedRepresentation18));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor,
                    InformAttributeReplacedRepresentation18));
            }
        }

        public void Start()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.Start();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, StartRepresentation19);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IClusterApplication>(_actor, consumer, StartRepresentation19));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StartRepresentation19));
            }
        }

        public void Conclude()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.Conclude();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, ConcludeRepresentation20);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IClusterApplication>(_actor, consumer, ConcludeRepresentation20));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ConcludeRepresentation20));
            }
        }

        public void Stop()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.Stop();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, StopRepresentation21);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IClusterApplication>(_actor, consumer, StopRepresentation21));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StopRepresentation21));
            }
        }
    }
}