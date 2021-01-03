// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Actors;

namespace Vlingo.Cluster.Model
{
    public class ClusterSnapshotControl__Proxy : IClusterSnapshotControl
    {
        private const string ShutDownRepresentation1 = "ShutDown()";

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public ClusterSnapshotControl__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public void ShutDown()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterSnapshotControl> consumer = x => x.ShutDown();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, ShutDownRepresentation1);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterSnapshotControl>(_actor, consumer, ShutDownRepresentation1));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, ShutDownRepresentation1));
            }
        }
    }
}