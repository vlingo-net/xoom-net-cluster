/*using System;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Message;

namespace Vlingo.Cluster.Model.Node
{
    using Vlingo.Wire.Node;

    public class LocalLiveNode__Proxy : ILocalLiveNode
    {
        private const string ConcludeRepresentation0 = "Conclude()";
        private const string HandleRepresentation1 = "Handle(OperationalMessage)";
        private const string RegisterNodeSynchronizerRepresentation2 = "RegisterNodeSynchronizer(INodeSynchronizer)";
        private const string StopRepresentation3 = "Stop()";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public LocalLiveNode__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public bool IsStopped => false;

        public void Handle(OperationalMessage message)
        {
            if (!actor.IsStopped)
            {
                Action<ILocalLiveNode> consumer = x => x.Handle(message);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, HandleRepresentation1);
                }
                else
                {
                    mailbox.Send(new LocalMessage<ILocalLiveNode>(actor, consumer, HandleRepresentation1));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, HandleRepresentation1));
            }
        }

        public void RegisterNodeSynchronizer(INodeSynchronizer nodeSynchronizer)
        {
            if (!actor.IsStopped)
            {
                Action<ILocalLiveNode> consumer = x => x.RegisterNodeSynchronizer(nodeSynchronizer);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, RegisterNodeSynchronizerRepresentation2);
                }
                else
                {
                    mailbox.Send(new LocalMessage<ILocalLiveNode>(actor, consumer,
                        RegisterNodeSynchronizerRepresentation2));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, RegisterNodeSynchronizerRepresentation2));
            }
        }

        public void Conclude()
        {
            if (!actor.IsStopped)
            {
                Action<ILocalLiveNode> consumer = __ => __.Conclude();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, ConcludeRepresentation0);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<ILocalLiveNode>(actor, consumer, ConcludeRepresentation0));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, ConcludeRepresentation0));
            }
        }

        public void Stop()
        {
            if (!actor.IsStopped)
            {
                Action<ILocalLiveNode> consumer = x => x.Stop();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, StopRepresentation3);
                }
                else
                {
                    mailbox.Send(new LocalMessage<ILocalLiveNode>(actor, consumer, StopRepresentation3));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, StopRepresentation3));
            }
        }
    }
}*/