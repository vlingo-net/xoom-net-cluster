// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Common;

namespace Vlingo.Cluster.Model.Node
{
    public class RegisteredNodeStatus
    {
        private bool _confirmedByLeader;
        private long _lastHealthIndication;
        private bool _leader;
        private readonly Xoom.Wire.Node.Node _node;

        public RegisteredNodeStatus(Xoom.Wire.Node.Node node, bool isLeader, bool confirmedByLeader)
        {
            _node = node;
            _leader = isLeader;
            _lastHealthIndication = DateTimeHelper.CurrentTimeMillis();
            _confirmedByLeader = confirmedByLeader;
        }

        public void ConfirmedByLeader(bool isConfirmed) => _confirmedByLeader = isConfirmed;

        public bool IsTimedOut(long currentTime, long liveNodeTimeout)
        {
            var timeOutTime = LastHealthIndication + liveNodeTimeout;
            return timeOutTime < currentTime;
        }

        public void UpdateLastHealthIndication() => _lastHealthIndication = DateTimeHelper.CurrentTimeMillis();

        public void SetLastHealthIndication(long millis) => _lastHealthIndication = millis;

        public void Lead(bool lead) => _leader = lead;
        
        public bool IsConfirmedByLeader => _confirmedByLeader;

        public bool IsLeader => _leader;

        public long LastHealthIndication => _lastHealthIndication;

        public Xoom.Wire.Node.Node Node => _node;
    }
}