// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Cluster.Model.Node
{
    internal sealed class TimeoutTracker
    {
        private bool _cleared = false;
        private long _startTime = -1L;
        private readonly long _timeout;

        internal TimeoutTracker(long timeout)
        {
            _timeout = timeout;
        }

        internal bool HasTimedOut
        {
            get
            {
                if (!_cleared && _startTime > 0)
                {
                    var currentTime = DateTimeHelper.CurrentTimeMillis();
                    return currentTime >= _startTime + _timeout;
                }

                return false;
            }
        }

        internal bool HasStarted => _startTime > 0;

        internal void Clear()
        {
            _cleared = true;
            Reset();
        }

        internal void Reset()
        {
            _startTime = -1L;
        }

        internal void Start() => Start(false);

        internal void Start(bool force)
        {
            if (force && _startTime == -1L || !_cleared && _startTime == -1L)
            {
                _cleared = false;
                _startTime = DateTimeHelper.CurrentTimeMillis();
            }
        }
    }
}