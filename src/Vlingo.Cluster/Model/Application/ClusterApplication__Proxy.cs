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

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public ClusterApplication__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public bool IsStopped => false;

        public void HandleApplicationMessage(RawMessage message, IApplicationOutboundStream? responder)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.HandleApplicationMessage(message, responder);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, HandleApplicationMessageRepresentation1);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        HandleApplicationMessageRepresentation1));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor,
                    HandleApplicationMessageRepresentation1));
            }
        }

        public void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAllLiveNodes(liveNodes, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformAllLiveNodesRepresentation2);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformAllLiveNodesRepresentation2));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformAllLiveNodesRepresentation2));
            }
        }

        public void InformLeaderElected(Id leaderId, bool isHealthyCluster, bool isLocalNodeLeading)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ =>
                    __.InformLeaderElected(leaderId, isHealthyCluster, isLocalNodeLeading);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformLeaderElectedRepresentation3);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformLeaderElectedRepresentation3));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformLeaderElectedRepresentation3));
            }
        }

        public void InformLeaderLost(Id lostLeaderId, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformLeaderLost(lostLeaderId, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformLeaderLostRepresentation4);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformLeaderLostRepresentation4));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformLeaderLostRepresentation4));
            }
        }

        public void InformLocalNodeShutDown(Id nodeId)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformLocalNodeShutDown(nodeId);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformLocalNodeShutDownRepresentation5);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformLocalNodeShutDownRepresentation5));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(
                    new DeadLetter(actor, InformLocalNodeShutDownRepresentation5));
            }
        }

        public void InformLocalNodeStarted(Id nodeId)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformLocalNodeStarted(nodeId);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformLocalNodeStartedRepresentation6);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformLocalNodeStartedRepresentation6));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor,
                    InformLocalNodeStartedRepresentation6));
            }
        }

        public void InformNodeIsHealthy(Id nodeId, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformNodeIsHealthy(nodeId, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformNodeIsHealthyRepresentation7);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformNodeIsHealthyRepresentation7));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformNodeIsHealthyRepresentation7));
            }
        }

        public void InformNodeJoinedCluster(Id nodeId, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformNodeJoinedCluster(nodeId, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformNodeJoinedClusterRepresentation8);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformNodeJoinedClusterRepresentation8));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(
                    new DeadLetter(actor, InformNodeJoinedClusterRepresentation8));
            }
        }

        public void InformNodeLeftCluster(Id nodeId, bool isHealthyCluster)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformNodeLeftCluster(nodeId, isHealthyCluster);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformNodeLeftClusterRepresentation9);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformNodeLeftClusterRepresentation9));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformNodeLeftClusterRepresentation9));
            }
        }

        public void InformQuorumAchieved()
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformQuorumAchieved();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformQuorumAchievedRepresentation10);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformQuorumAchievedRepresentation10));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformQuorumAchievedRepresentation10));
            }
        }

        public void InformQuorumLost()
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformQuorumLost();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformQuorumLostRepresentation11);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformQuorumLostRepresentation11));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformQuorumLostRepresentation11));
            }
        }

        public void InformAttributesClient(IAttributesProtocol client)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributesClient(client);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformAttributesClientRepresentation12);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformAttributesClientRepresentation12));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(
                    new DeadLetter(actor, InformAttributesClientRepresentation12));
            }
        }

        public void InformAttributeSetCreated(string? attributeSetName)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeSetCreated(attributeSetName);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformAttributeSetCreatedRepresentation13);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformAttributeSetCreatedRepresentation13));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor,
                    InformAttributeSetCreatedRepresentation13));
            }
        }

        public void InformAttributeAdded(string attributeSetName, string? attributeName)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeAdded(attributeSetName, attributeName);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformAttributeAddedRepresentation14);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformAttributeAddedRepresentation14));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, InformAttributeAddedRepresentation14));
            }
        }

        public void InformAttributeRemoved(string attributeSetName, string? attributeName)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeRemoved(attributeSetName, attributeName);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformAttributeRemovedRepresentation15);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformAttributeRemovedRepresentation15));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(
                    new DeadLetter(actor, InformAttributeRemovedRepresentation15));
            }
        }

        public void InformAttributeSetRemoved(string? attributeSetName)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.InformAttributeSetRemoved(attributeSetName);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformAttributeSetRemovedRepresentation16);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformAttributeSetRemovedRepresentation16));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor,
                    InformAttributeSetRemovedRepresentation16));
            }
        }

        public void InformAttributeReplaced(string attributeSetName, string? attributeName)
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ =>
                    __.InformAttributeReplaced(attributeSetName, attributeName);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, InformAttributeReplacedRepresentation17);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterApplication>(actor, consumer,
                        InformAttributeReplacedRepresentation17));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor,
                    InformAttributeReplacedRepresentation17));
            }
        }

        public void Start()
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.Start();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, StartRepresentation18);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IClusterApplication>(actor, consumer, StartRepresentation18));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, StartRepresentation18));
            }
        }

        public void Conclude()
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.Conclude();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, ConcludeRepresentation19);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IClusterApplication>(actor, consumer, ConcludeRepresentation19));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, ConcludeRepresentation19));
            }
        }

        public void Stop()
        {
            if (!actor.IsStopped)
            {
                Action<IClusterApplication> consumer = __ => __.Stop();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, StopRepresentation20);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IClusterApplication>(actor, consumer, StopRepresentation20));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, StopRepresentation20));
            }
        }
    }
}