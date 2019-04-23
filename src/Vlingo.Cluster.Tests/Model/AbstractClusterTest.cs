// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Actors.TestKit;
using Vlingo.Common;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model
{
    using Vlingo.Cluster.Model;
    
    public class AbstractClusterTest : AbstractMessageTool, IDisposable
    {
        private static readonly Random Random = new Random();
        private static AtomicInteger _portToUse = new AtomicInteger(10_000 + Random.Next(50_000));
        
        protected MockClusterApplication Application;
        protected Properties Properties;
        protected TestWorld TestWorld;

        public AbstractClusterTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);
        }

        public void Dispose()
        {
            TestWorld?.Terminate();
            Cluster.Reset();
        }
    }
}