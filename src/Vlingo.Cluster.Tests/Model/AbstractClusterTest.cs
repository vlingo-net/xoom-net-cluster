// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Common;

namespace Vlingo.Cluster.Tests.Model
{
    public class AbstractClusterTest : AbstractMessageTool
    {
        private static readonly Random Random = new Random();
        private static AtomicInteger _portToUse = new AtomicInteger(10_000 + Random.Next(50_000));
    }
}