// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Attribute;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;

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
        private const string InformAttributesClientRepresentation12 = "InformAttributesClient(IAttributesProtocol)";
        private const string InformAttributeSetCreatedRepresentation13 = "InformAttributeSetCreated(string)";
        private const string InformAttributeAddedRepresentation14 = "InformAttributeAdded(string, string)";
        private const string InformAttributeRemovedRepresentation15 = "InformAttributeRemoved(string, string)";
        private const string InformAttributeSetRemovedRepresentation16 = "InformAttributeSetRemoved(string)";
        private const string InformAttributeReplacedRepresentation17 = "InformAttributeReplaced(string, string)";
        private const string StartRepresentation18 = "Start()";
        private const string ConcludeRepresentation19 = "Conclude()";
        private const string StopRepresentation20 = "Stop()";

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public ClusterApplication__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public bool IsStopped => false;

        public void HandleApplicationMessage(RawMessage message, IApplicationOutboundStream? responder)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.HandleApplicationMessage(message, responder);
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

        public void InformAttributesClient(IAttributesProtocol client)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributesClient(client);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributesClientRepresentation12);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributesClientRepresentation12));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(
                    new DeadLetter(_actor, InformAttributesClientRepresentation12));
            }
        }

        public void InformAttributeSetCreated(string? attributeSetName)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeSetCreated(attributeSetName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributeSetCreatedRepresentation13);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeSetCreatedRepresentation13));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor,
                    InformAttributeSetCreatedRepresentation13));
            }
        }

        public void InformAttributeAdded(string attributeSetName, string? attributeName)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeAdded(attributeSetName, attributeName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributeAddedRepresentation14);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeAddedRepresentation14));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformAttributeAddedRepresentation14));
            }
        }

        public void InformAttributeRemoved(string attributeSetName, string? attributeName)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeRemoved(attributeSetName, attributeName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributeRemovedRepresentation15);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeRemovedRepresentation15));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(
                    new DeadLetter(_actor, InformAttributeRemovedRepresentation15));
            }
        }

        public void InformAttributeSetRemoved(string? attributeSetName)
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeSetRemoved(attributeSetName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAttributeSetRemovedRepresentation16);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeSetRemovedRepresentation16));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor,
                    InformAttributeSetRemovedRepresentation16));
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
                    _mailbox.Send(_actor, consumer, null, InformAttributeReplacedRepresentation17);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterApplication>(_actor, consumer,
                        InformAttributeReplacedRepresentation17));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor,
                    InformAttributeReplacedRepresentation17));
            }
        }

        public void Start()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.Start();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, StartRepresentation18);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IClusterApplication>(_actor, consumer, StartRepresentation18));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StartRepresentation18));
            }
        }

        public void Conclude()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.Conclude();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, ConcludeRepresentation19);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IClusterApplication>(_actor, consumer, ConcludeRepresentation19));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ConcludeRepresentation19));
            }
        }

        public void Stop()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.Stop();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, StopRepresentation20);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IClusterApplication>(_actor, consumer, StopRepresentation20));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StopRepresentation20));
            }
        }
    }
}