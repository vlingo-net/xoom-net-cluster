// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Cluster.Model.Message;
using Vlingo.Xoom.Actors;

namespace Vlingo.Cluster.Model.Outbound
{
    using Vlingo.Wire.Node;

    public class OperationalOutboundStream__Proxy : IOperationalOutboundStream
    {
        private const string CloseRepresentation1 = "Close(Id)";
        private const string ApplicationRepresentation2 = "Application(ApplicationSays, IEnumerable<Node>)";
        private const string DirectoryRepresentation3 = "Directory(IEnumerable<Node>)";
        private const string ElectRepresentation4 = "Elect(IEnumerable<Node>)";
        private const string JoinRepresentation5 = "Join()";
        private const string LeaderRepresentation6 = "Leader()";
        private const string LeaderRepresentation7 = "Leader(Id)";
        private const string LeaveRepresentation8 = "Leave()";
        private const string OpenRepresentation9 = "Open(Id)";
        private const string PingRepresentation10 = "Ping(Id)";
        private const string PulseRepresentation11 = "Pulse(Id)";
        private const string PulseRepresentation12 = "Pulse()";
        private const string SplitRepresentation13 = "Split(Id, Id)";
        private const string VoteRepresentation14 = "Vote(Id)";
        private const string ConcludeRepresentation15 = "Conclude()";
        private const string StopRepresentation16 = "Stop()";

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public OperationalOutboundStream__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public bool IsStopped => false;

        public void Close(Id id)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Close(id);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, CloseRepresentation1);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, CloseRepresentation1));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, CloseRepresentation1));
            }
        }

        public void Application(ApplicationSays says, IEnumerable<Node> unconfirmedNodes)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Application(says, unconfirmedNodes);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, ApplicationRepresentation2);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IOperationalOutboundStream>(_actor, consumer,
                        ApplicationRepresentation2));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ApplicationRepresentation2));
            }
        }

        public void Directory(IEnumerable<Node> allLiveNodes)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Directory(allLiveNodes);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, DirectoryRepresentation3);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, DirectoryRepresentation3));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, DirectoryRepresentation3));
            }
        }

        public void Elect(IEnumerable<Node> allGreaterNodes)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Elect(allGreaterNodes);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, ElectRepresentation4);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, ElectRepresentation4));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ElectRepresentation4));
            }
        }

        public void Join()
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Join();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, JoinRepresentation5);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, JoinRepresentation5));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, JoinRepresentation5));
            }
        }

        public void Leader()
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Leader();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, LeaderRepresentation6);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, LeaderRepresentation6));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, LeaderRepresentation6));
            }
        }

        public void Leader(Id id)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Leader(id);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, LeaderRepresentation7);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, LeaderRepresentation7));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, LeaderRepresentation7));
            }
        }

        public void Leave()
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Leave();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, LeaveRepresentation8);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, LeaveRepresentation8));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, LeaveRepresentation8));
            }
        }

        public void Open(Id id)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Open(id);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, OpenRepresentation9);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, OpenRepresentation9));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, OpenRepresentation9));
            }
        }

        public void Ping(Id targetNodeId)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Ping(targetNodeId);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, PingRepresentation10);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, PingRepresentation10));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PingRepresentation10));
            }
        }

        public void Pulse(Id targetNodeId)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Pulse(targetNodeId);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, PulseRepresentation11);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, PulseRepresentation11));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PulseRepresentation11));
            }
        }

        public void Pulse()
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Pulse();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, PulseRepresentation12);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, PulseRepresentation12));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, PulseRepresentation12));
            }
        }

        public void Split(Id targetNodeId, Id currentLeaderId)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Split(targetNodeId, currentLeaderId);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, SplitRepresentation13);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, SplitRepresentation13));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, SplitRepresentation13));
            }
        }

        public void Vote(Id targetNodeId)
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Vote(targetNodeId);
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, VoteRepresentation14);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, VoteRepresentation14));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, VoteRepresentation14));
            }
        }

        public void Conclude()
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Conclude();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, ConcludeRepresentation15);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, ConcludeRepresentation15));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ConcludeRepresentation15));
            }
        }

        public void Stop()
        {
            if (!_actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Stop();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, StopRepresentation16);
                }
                else
                {
                    _mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(_actor, consumer, StopRepresentation16));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, StopRepresentation16));
            }
        }
    }
}