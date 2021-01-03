// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors.TestKit;
using Vlingo.Common;
using Vlingo.Wire.Node;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model
{
    using Vlingo.Cluster.Model;

    public class AbstractClusterTest : AbstractMessageTool, IDisposable
    {
        private static readonly Random Random = new Random();
        private static readonly AtomicInteger PortToUse = new AtomicInteger(10_000 + Random.Next(50_000));

        protected readonly MockClusterApplication Application;
        protected readonly Properties Properties;
        protected readonly TestWorld TestWorld;

        [Fact]
        public void TestValues()
        {
            Assert.NotNull(Application);
            Assert.NotNull(Config);
            Assert.NotNull(Properties);
            Assert.NotNull(TestWorld);
        }

        public AbstractClusterTest(ITestOutputHelper output)
        {
            var converter = new Converter(output);
            Console.SetOut(converter);

            var properties = new Dictionary<string, string>();

            properties.Add("cluster.ssl", "false");

            properties.Add("cluster.op.buffer.size", "4096");
            properties.Add("cluster.app.buffer.size", "10240");
            properties.Add("cluster.op.outgoing.pooled.buffers", "20");
            properties.Add("cluster.app.outgoing.pooled.buffers", "50");

            properties.Add("cluster.msg.charset", "UTF-8");

            properties.Add("cluster.app.class", "Vlingo.Cluster.Model.Application.FakeClusterApplicationActor");

            properties.Add("cluster.health.check.interval", "2000");
            properties.Add("cluster.live.node.timeout", "20000");
            properties.Add("cluster.heartbeat.interval", "7000");
            properties.Add("cluster.quorum.timeout", "60000");

            properties.Add("cluster.seedNodes", "node1,node2,node3");

            properties.Add("node.node1.id", "1");
            properties.Add("node.node1.name", "node1");
            properties.Add("node.node1.host", "localhost");
            properties.Add("node.node1.op.port", NextPortToUseString());
            properties.Add("node.node1.app.port", NextPortToUseString());

            properties.Add("node.node2.id", "2");
            properties.Add("node.node2.name", "node2");
            properties.Add("node.node2.host", "localhost");
            properties.Add("node.node2.op.port", NextPortToUseString());
            properties.Add("node.node2.app.port", NextPortToUseString());

            properties.Add("node.node3.id", "3");
            properties.Add("node.node3.name", "node3");
            properties.Add("node.node3.host", "localhost");
            properties.Add("node.node3.op.port", NextPortToUseString());
            properties.Add("node.node3.app.port", NextPortToUseString());

            Properties = Properties.Instance;
            Properties.SetCustomProperties(properties);

            TestWorld = TestWorld.Start("cluster-test-world");

            Config = new ClusterConfiguration(Properties, TestWorld.DefaultLogger);

            Application = new MockClusterApplication();
        }

        public virtual void Dispose() => TestWorld?.Terminate();

        protected Wire.Node.Node NextNodeWith(int nodeNumber) => Wire.Node.Node.With(Id.Of(nodeNumber),
            Name.Of($"node{nodeNumber}"), Host.Of("localhost"), NextPortToUse(), NextPortToUse());

        private int NextPortToUse() => PortToUse.GetAndIncrement();

        private string NextPortToUseString() => NextPortToUse().ToString();
    }
}