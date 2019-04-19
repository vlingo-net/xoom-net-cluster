// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Cluster.Model.Node
{
    using Vlingo.Wire.Node;
    
    public class RegisteredNodeStatus
    {
        private static readonly DateTime Jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private bool _confirmedByLeader;
        private long _lastHealthIndication;
        private bool _leader;
        private readonly Node _node;
        
        public static long CurrentTimeMillis()
        {
            return (long) (DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public RegisteredNodeStatus(Node node, bool isLeader, bool confirmedByLeader)
        {
            _node = node;
            _leader = isLeader;
            _lastHealthIndication = CurrentTimeMillis();
            _confirmedByLeader = confirmedByLeader;
        }

        public void ConfirmedByLeader(bool isConfirmed) => _confirmedByLeader = isConfirmed;

        public bool IsTimedOut(long currentTime, long liveNodeTimeout)
        {
            var timeOutTime = LastHealthIndication + liveNodeTimeout;
            return timeOutTime < currentTime;
        }

        public void UpdateLastHealthIndication() => _lastHealthIndication = CurrentTimeMillis();

        public void SetLastHealthIndication(long millis) => _lastHealthIndication = millis;

        public void Lead(bool lead) => _leader = lead;
        
        public bool IsConfirmedByLeader => _confirmedByLeader;

        public bool IsLeader => _leader;

        public long LastHealthIndication => _lastHealthIndication;

        public Node Node => _node;
    }
}