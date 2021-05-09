// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Xoom.Actors;

namespace Vlingo.Xoom.Cluster.Model.Nodes
{
    public class RegistryInterest__Proxy : IRegistryInterest
    {
        private const string InformAllLiveNodesRepresentation1 = "InformAllLiveNodes(IEnumerable<Node>, bool)";
        private const string InformConfirmedByLeaderRepresentation2 = "InformConfirmedByLeader(Node, bool)";
        private const string InformCurrentLeaderRepresentation3 = "InformCurrentLeader(Node, bool)";

        private const string InformMergedAllDirectoryEntriesRepresentation4 =
            "InformMergedAllDirectoryEntries(IEnumerable<Node>, IEnumerable<MergeResult>, bool)";

        private const string InformLeaderDemotedRepresentation5 = "InformLeaderDemoted(Node, bool)";
        private const string InformNodeIsHealthyRepresentation6 = "InformNodeIsHealthy(Node, bool)";
        private const string InformNodeJoinedClusterRepresentation7 = "InformNodeJoinedCluster(Node, bool)";
        private const string InformNodeLeftClusterRepresentation8 = "InformNodeLeftCluster(Node, bool)";
        private const string InformNodeTimedOutRepresentation9 = "InformNodeTimedOut(Node, bool)";

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public RegistryInterest__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public void InformAllLiveNodes(IEnumerable<Xoom.Wire.Nodes.Node> liveNodes, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformAllLiveNodes(liveNodes, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformAllLiveNodesRepresentation1);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IRegistryInterest>(_actor, consumer, InformAllLiveNodesRepresentation1));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformAllLiveNodesRepresentation1));
            }
        }

        public void InformConfirmedByLeader(Xoom.Wire.Nodes.Node node, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformConfirmedByLeader(node, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformConfirmedByLeaderRepresentation2);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IRegistryInterest>(_actor, consumer,
                        InformConfirmedByLeaderRepresentation2));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformConfirmedByLeaderRepresentation2));
            }
        }

        public void InformCurrentLeader(Xoom.Wire.Nodes.Node node, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformCurrentLeader(node, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformCurrentLeaderRepresentation3);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IRegistryInterest>(_actor, consumer,
                        InformCurrentLeaderRepresentation3));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformCurrentLeaderRepresentation3));
            }
        }

        public void InformMergedAllDirectoryEntries(IEnumerable<Xoom.Wire.Nodes.Node> liveNodes, IEnumerable<MergeResult> mergeResults,
            bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x =>
                    x.InformMergedAllDirectoryEntries(liveNodes, mergeResults, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformMergedAllDirectoryEntriesRepresentation4);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IRegistryInterest>(_actor, consumer,
                        InformMergedAllDirectoryEntriesRepresentation4));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformMergedAllDirectoryEntriesRepresentation4));
            }
        }

        public void InformLeaderDemoted(Xoom.Wire.Nodes.Node node, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformLeaderDemoted(node, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformLeaderDemotedRepresentation5);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IRegistryInterest>(_actor, consumer,
                        InformLeaderDemotedRepresentation5));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformLeaderDemotedRepresentation5));
            }
        }

        public void InformNodeIsHealthy(Xoom.Wire.Nodes.Node node, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformNodeIsHealthy(node, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformNodeIsHealthyRepresentation6);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IRegistryInterest>(_actor, consumer,
                        InformNodeIsHealthyRepresentation6));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformNodeIsHealthyRepresentation6));
            }
        }

        public void InformNodeJoinedCluster(Xoom.Wire.Nodes.Node node, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformNodeJoinedCluster(node, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformNodeJoinedClusterRepresentation7);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IRegistryInterest>(_actor, consumer,
                        InformNodeJoinedClusterRepresentation7));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformNodeJoinedClusterRepresentation7));
            }
        }

        public void InformNodeLeftCluster(Xoom.Wire.Nodes.Node node, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformNodeLeftCluster(node, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformNodeLeftClusterRepresentation8);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IRegistryInterest>(_actor, consumer,
                        InformNodeLeftClusterRepresentation8));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformNodeLeftClusterRepresentation8));
            }
        }

        public void InformNodeTimedOut(Xoom.Wire.Nodes.Node node, bool isHealthyCluster)
        {
            if (!_actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformNodeTimedOut(node, isHealthyCluster);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, InformNodeTimedOutRepresentation9);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IRegistryInterest>(_actor, consumer, InformNodeTimedOutRepresentation9));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, InformNodeTimedOutRepresentation9));
            }
        }
    }
}