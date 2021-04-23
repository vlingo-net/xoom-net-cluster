// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Xoom.Actors;

namespace Vlingo.Cluster.Model
{
    public class ClusterSnapshot__Proxy : IClusterSnapshot
    {
        private const string QuorumAchievedRepresentation1 = "QuorumAchieved()";
        private const string QuorumLostRepresentation2 = "QuorumLost()";

        private readonly Actor _actor;
        private readonly IMailbox _mailbox;

        public ClusterSnapshot__Proxy(Actor actor, IMailbox mailbox)
        {
            _actor = actor;
            _mailbox = mailbox;
        }

        public void QuorumAchieved()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterSnapshot> consumer = x => x.QuorumAchieved();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, QuorumAchievedRepresentation1);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterSnapshot>(_actor, consumer, QuorumAchievedRepresentation1));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, QuorumAchievedRepresentation1));
            }
        }

        public void QuorumLost()
        {
            if (!_actor.IsStopped)
            {
                Action<IClusterSnapshot> consumer = x => x.QuorumLost();
                if (_mailbox.IsPreallocated)
                {
                    _mailbox.Send(_actor, consumer, null, QuorumLostRepresentation2);
                }
                else
                {
                    _mailbox.Send(new LocalMessage<IClusterSnapshot>(_actor, consumer, QuorumLostRepresentation2));
                }
            }
            else
            {
                _actor.DeadLetters?.FailedDelivery(new DeadLetter(_actor, QuorumLostRepresentation2));
            }
        }
    }
}