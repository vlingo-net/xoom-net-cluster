using System;
using Vlingo.Actors;
using Vlingo.Common;
using Vlingo.Wire.Message;

namespace Vlingo.Cluster.Model.Attribute
{
    using Vlingo.Wire.Node;
    
    public class AttributesAgent__Proxy : IAttributesAgent
    {
        private const string RepresentationConclude0 = "Conclude()";
        private const string AddRepresentation1 = "Add(string, string, T)";
        private const string ReplaceRepresentation2 = "Replace(string, string, T)";
        private const string RemoveRepresentation3 = "Remove(string, string)";
        private const string RemoveAllRepresentation4 = "RemoveAll(string)";
        private const string SynchronizeRepresentation5 = "Synchronize(Node)";

        private const string HandleInboundStreamMessageRepresentation6 =
            "HandleInboundStreamMessage(AddressType, RawMessage)";

        private const string IntervalSignalRepresentation7 = "IntervalSignal(IScheduled<object>, object)";
        private const string StopRepresentation8 = "Stop()";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public AttributesAgent__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public bool IsStopped => false;

        public void Add<T>(string attributeSetName, string attributeName, T value)
        {
            if (!actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Add(attributeSetName, attributeName, value);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, AddRepresentation1);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IAttributesAgent>(actor, consumer, AddRepresentation1));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, AddRepresentation1));
            }
        }

        public void Replace<T>(string attributeSetName, string attributeName, T value)
        {
            if (!actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Replace(attributeSetName, attributeName, value);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, ReplaceRepresentation2);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IAttributesAgent>(actor, consumer, ReplaceRepresentation2));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, ReplaceRepresentation2));
            }
        }

        public void Remove(string attributeSetName, string attributeName)
        {
            if (!actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Remove(attributeSetName, attributeName);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, RemoveRepresentation3);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IAttributesAgent>(actor, consumer, RemoveRepresentation3));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, RemoveRepresentation3));
            }
        }

        public void RemoveAll(string attributeSetName)
        {
            if (!actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.RemoveAll(attributeSetName);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, RemoveAllRepresentation4);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IAttributesAgent>(actor, consumer, RemoveAllRepresentation4));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, RemoveAllRepresentation4));
            }
        }

        public void Synchronize(Node node)
        {
            if (!actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Synchronize(node);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, SynchronizeRepresentation5);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IAttributesAgent>(actor, consumer, SynchronizeRepresentation5));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, SynchronizeRepresentation5));
            }
        }

        public void HandleInboundStreamMessage(AddressType addressType, RawMessage message)
        {
            if (!actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.HandleInboundStreamMessage(addressType, message);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, HandleInboundStreamMessageRepresentation6);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IAttributesAgent>(actor, consumer,
                        HandleInboundStreamMessageRepresentation6));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, HandleInboundStreamMessageRepresentation6));
            }
        }

        public void IntervalSignal(IScheduled<object> scheduled, object data)
        {
            if (!actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.IntervalSignal(scheduled, data);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, IntervalSignalRepresentation7);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IAttributesAgent>(actor, consumer, IntervalSignalRepresentation7));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, IntervalSignalRepresentation7));
            }
        }
        
        public void Conclude()
        {
            if (!actor.IsStopped)
            {
                Action<IStoppable> consumer = x => x.Conclude();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, RepresentationConclude0);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IStoppable>(actor, consumer, RepresentationConclude0));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, RepresentationConclude0));
            }
        }

        public void Stop()
        {
            if (!actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Stop();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, StopRepresentation8);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IAttributesAgent>(actor, consumer, StopRepresentation8));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, StopRepresentation8));
            }
        }
    }
}