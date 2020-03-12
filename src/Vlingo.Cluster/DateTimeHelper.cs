// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Cluster
{
    public static class DateTimeHelper
    {
        private static readonly DateTime Jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public static long CurrentTimeMillis()
        {
            return (long) (DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }
    }
}