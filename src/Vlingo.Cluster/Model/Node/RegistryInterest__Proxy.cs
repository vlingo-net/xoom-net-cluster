using System;
using System.Collections.Generic;
using Vlingo.Actors;

namespace Vlingo.Cluster.Model.Node
{
    using Vlingo.Wire.Node;

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

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public RegistryInterest__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformAllLiveNodes(liveNodes, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformAllLiveNodesRepresentation1);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IRegistryInterest>(actor, consumer, InformAllLiveNodesRepresentation1));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformAllLiveNodesRepresentation1));
            }
        }

        public void InformConfirmedByLeader(Node node, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformConfirmedByLeader(node, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformConfirmedByLeaderRepresentation2);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IRegistryInterest>(actor, consumer,
                        InformConfirmedByLeaderRepresentation2));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformConfirmedByLeaderRepresentation2));
            }
        }

        public void InformCurrentLeader(Node node, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformCurrentLeader(node, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformCurrentLeaderRepresentation3);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IRegistryInterest>(actor, consumer,
                        InformCurrentLeaderRepresentation3));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformCurrentLeaderRepresentation3));
            }
        }

        public void InformMergedAllDirectoryEntries(IEnumerable<Node> liveNodes, IEnumerable<MergeResult> mergeResults,
            bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x =>
                    x.InformMergedAllDirectoryEntries(liveNodes, mergeResults, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformMergedAllDirectoryEntriesRepresentation4);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IRegistryInterest>(actor, consumer,
                        InformMergedAllDirectoryEntriesRepresentation4));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformMergedAllDirectoryEntriesRepresentation4));
            }
        }

        public void InformLeaderDemoted(Node node, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformLeaderDemoted(node, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformLeaderDemotedRepresentation5);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IRegistryInterest>(actor, consumer,
                        InformLeaderDemotedRepresentation5));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformLeaderDemotedRepresentation5));
            }
        }

        public void InformNodeIsHealthy(Node node, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformNodeIsHealthy(node, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformNodeIsHealthyRepresentation6);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IRegistryInterest>(actor, consumer,
                        InformNodeIsHealthyRepresentation6));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformNodeIsHealthyRepresentation6));
            }
        }

        public void InformNodeJoinedCluster(Node node, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformNodeJoinedCluster(node, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformNodeJoinedClusterRepresentation7);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IRegistryInterest>(actor, consumer,
                        InformNodeJoinedClusterRepresentation7));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformNodeJoinedClusterRepresentation7));
            }
        }

        public void InformNodeLeftCluster(Node node, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformNodeLeftCluster(node, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformNodeLeftClusterRepresentation8);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IRegistryInterest>(actor, consumer,
                        InformNodeLeftClusterRepresentation8));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformNodeLeftClusterRepresentation8));
            }
        }

        public void InformNodeTimedOut(Node node, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IRegistryInterest> consumer = x => x.InformNodeTimedOut(node, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformNodeTimedOutRepresentation9);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IRegistryInterest>(actor, consumer, InformNodeTimedOutRepresentation9));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformNodeTimedOutRepresentation9));
            }
        }
    }
}