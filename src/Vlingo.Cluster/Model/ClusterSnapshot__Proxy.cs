using System;
using Vlingo.Actors;

namespace Vlingo.Cluster.Model
{
    public class ClusterSnapshot__Proxy : IClusterSnapshot
    {
        private const string QuorumAchievedRepresentation1 = "QuorumAchieved()";
        private const string QuorumLostRepresentation2 = "QuorumLost()";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public ClusterSnapshot__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public void QuorumAchieved()
        {
            if (!actor.IsStopped)
            {
                Action<IClusterSnapshot> consumer = x => x.QuorumAchieved();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, QuorumAchievedRepresentation1);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterSnapshot>(actor, consumer, QuorumAchievedRepresentation1));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, QuorumAchievedRepresentation1));
            }
        }

        public void QuorumLost()
        {
            if (!actor.IsStopped)
            {
                Action<IClusterSnapshot> consumer = x => x.QuorumLost();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, QuorumLostRepresentation2);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterSnapshot>(actor, consumer, QuorumLostRepresentation2));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, QuorumLostRepresentation2));
            }
        }
    }
}