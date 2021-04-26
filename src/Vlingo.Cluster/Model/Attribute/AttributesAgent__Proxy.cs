// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Cluster.Model.Attribute
{
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

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public AttributesAgent__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public bool IsStopped => false;

        public void Add<T>(string attributeSetName, string attributeName, T value)
        {
            if (!_actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Add(attributeSetName, attributeName, value);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, AddRepresentation1);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IAttributesAgent>(_actor, consumer, AddRepresentation1));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, AddRepresentation1));
            }
        }

        public void Replace<T>(string attributeSetName, string attributeName, T value)
        {
            if (!_actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Replace(attributeSetName, attributeName, value);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, ReplaceRepresentation2);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IAttributesAgent>(_actor, consumer, ReplaceRepresentation2));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ReplaceRepresentation2));
            }
        }

        public void Remove(string attributeSetName, string attributeName)
        {
            if (!_actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Remove(attributeSetName, attributeName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, RemoveRepresentation3);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IAttributesAgent>(_actor, consumer, RemoveRepresentation3));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, RemoveRepresentation3));
            }
        }

        public void RemoveAll(string attributeSetName)
        {
            if (!_actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.RemoveAll(attributeSetName);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, RemoveAllRepresentation4);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IAttributesAgent>(_actor, consumer, RemoveAllRepresentation4));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, RemoveAllRepresentation4));
            }
        }

        public void Synchronize(Node node)
        {
            if (!_actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Synchronize(node);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, SynchronizeRepresentation5);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IAttributesAgent>(_actor, consumer, SynchronizeRepresentation5));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, SynchronizeRepresentation5));
            }
        }

        public void HandleInboundStreamMessage(AddressType addressType, RawMessage message)
        {
            if (!_actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.HandleInboundStreamMessage(addressType, message);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, HandleInboundStreamMessageRepresentation6);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IAttributesAgent>(_actor, consumer,
                        HandleInboundStreamMessageRepresentation6));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, HandleInboundStreamMessageRepresentation6));
            }
        }

        public void IntervalSignal(IScheduled<object> scheduled, object data)
        {
            if (!_actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.IntervalSignal(scheduled, data);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, IntervalSignalRepresentation7);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IAttributesAgent>(_actor, consumer, IntervalSignalRepresentation7));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, IntervalSignalRepresentation7));
            }
        }
        
        public void Conclude()
        {
            if (!_actor.IsStopped)
            {
                Action<IStoppable> consumer = x => x.Conclude();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, RepresentationConclude0);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IStoppable>(_actor, consumer, RepresentationConclude0));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, RepresentationConclude0));
            }
        }

        public void Stop()
        {
            if (!_actor.IsStopped)
            {
                Action<IAttributesAgent> consumer = x => x.Stop();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, StopRepresentation8);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IAttributesAgent>(_actor, consumer, StopRepresentation8));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StopRepresentation8));
            }
        }
    }
}