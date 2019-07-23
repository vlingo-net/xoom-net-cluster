using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Message;

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

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public OperationalOutboundStream__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public bool IsStopped => false;

        public void Close(Id id)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Close(id);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, CloseRepresentation1);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, CloseRepresentation1));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, CloseRepresentation1));
            }
        }

        public void Application(ApplicationSays says, IEnumerable<Node> unconfirmedNodes)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Application(says, unconfirmedNodes);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, ApplicationRepresentation2);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IOperationalOutboundStream>(actor, consumer,
                        ApplicationRepresentation2));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, ApplicationRepresentation2));
            }
        }

        public void Directory(IEnumerable<Node> allLiveNodes)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Directory(allLiveNodes);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, DirectoryRepresentation3);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, DirectoryRepresentation3));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, DirectoryRepresentation3));
            }
        }

        public void Elect(IEnumerable<Node> allGreaterNodes)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Elect(allGreaterNodes);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, ElectRepresentation4);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, ElectRepresentation4));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, ElectRepresentation4));
            }
        }

        public void Join()
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Join();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, JoinRepresentation5);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, JoinRepresentation5));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, JoinRepresentation5));
            }
        }

        public void Leader()
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Leader();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, LeaderRepresentation6);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, LeaderRepresentation6));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, LeaderRepresentation6));
            }
        }

        public void Leader(Id id)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Leader(id);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, LeaderRepresentation7);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, LeaderRepresentation7));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, LeaderRepresentation7));
            }
        }

        public void Leave()
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Leave();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, LeaveRepresentation8);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, LeaveRepresentation8));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, LeaveRepresentation8));
            }
        }

        public void Open(Id id)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Open(id);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, OpenRepresentation9);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, OpenRepresentation9));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, OpenRepresentation9));
            }
        }

        public void Ping(Id targetNodeId)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Ping(targetNodeId);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, PingRepresentation10);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, PingRepresentation10));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, PingRepresentation10));
            }
        }

        public void Pulse(Id targetNodeId)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Pulse(targetNodeId);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, PulseRepresentation11);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, PulseRepresentation11));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, PulseRepresentation11));
            }
        }

        public void Pulse()
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Pulse();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, PulseRepresentation12);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, PulseRepresentation12));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, PulseRepresentation12));
            }
        }

        public void Split(Id targetNodeId, Id currentLeaderId)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Split(targetNodeId, currentLeaderId);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, SplitRepresentation13);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, SplitRepresentation13));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, SplitRepresentation13));
            }
        }

        public void Vote(Id targetNodeId)
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Vote(targetNodeId);
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, VoteRepresentation14);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, VoteRepresentation14));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, VoteRepresentation14));
            }
        }

        public void Conclude()
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Conclude();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, ConcludeRepresentation15);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, ConcludeRepresentation15));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, ConcludeRepresentation15));
            }
        }

        public void Stop()
        {
            if (!actor.IsStopped)
            {
                Action<IOperationalOutboundStream> consumer = __ => __.Stop();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, StopRepresentation16);
                }
                else
                {
                    mailbox.Send(
                        new LocalMessage<IOperationalOutboundStream>(actor, consumer, StopRepresentation16));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, StopRepresentation16));
            }
        }
    }
}