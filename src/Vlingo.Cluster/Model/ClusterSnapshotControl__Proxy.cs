using System;
using Vlingo.Actors;

namespace Vlingo.Cluster.Model
{
    public class ClusterSnapshotControl__Proxy : IClusterSnapshotControl
    {
        private const string ShutDownRepresentation1 = "ShutDown()";

        private readonly Actor actor;
        private readonly IMailbox mailbox;

        public ClusterSnapshotControl__Proxy(Actor actor, IMailbox mailbox)
        {
            this.actor = actor;
            this.mailbox = mailbox;
        }

        public void ShutDown()
        {
            if (!actor.IsStopped)
            {
                Action<IClusterSnapshotControl> consumer = x => x.ShutDown();
                if (mailbox.IsPreallocated)
                {
                    mailbox.Send(actor, consumer, null, ShutDownRepresentation1);
                }
                else
                {
                    mailbox.Send(new LocalMessage<IClusterSnapshotControl>(actor, consumer, ShutDownRepresentation1));
                }
            }
            else
            {
                actor.DeadLetters.FailedDelivery(new DeadLetter(actor, ShutDownRepresentation1));
            }
        }
    }
}