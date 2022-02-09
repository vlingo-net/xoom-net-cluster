// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Text;
using Vlingo.Xoom.Cluster.Model;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Cluster
{
    /// <summary>
    /// Properties that are predefined for single-node and multi-node clusters. These
    /// are useful for test, but provided in main for access by tests outside of vlingo-net-cluster.
    /// </summary>
    public class ClusterProperties
    {
        private const string DefaultApplicationClassname = "Vlingo.Xoom.Cluster.Model.Application.FakeClusterApplicationActor";
        private static readonly Random Random = new Random();
        private static readonly AtomicInteger PortToUse = new AtomicInteger(10_000 + Random.Next(50_000));
        
        public static Properties AllNodes() => AllNodes(PortToUse);

        public static Properties AllNodes(AtomicInteger portSeed) => AllNodes(PortToUse, 3);

        public static Properties AllNodes(AtomicInteger portSeed, int totalNodes) => AllNodes(PortToUse, totalNodes, DefaultApplicationClassname);

        public static Properties AllNodes(AtomicInteger portSeed, int totalNodes, string applicationClassname)
        {
            var properties = new Properties();

            properties = Common(AllOf(properties, totalNodes, portSeed), totalNodes, applicationClassname);

            var clusterProperties = Properties.OpenWith(properties);

            return clusterProperties;
        }

        public static Properties OneNode() => OneNode(PortToUse);

        public static Properties OneNode(AtomicInteger portSeed) => OneNode(portSeed, DefaultApplicationClassname);

        public static Properties OneNode(AtomicInteger portSeed, string applicationClassname)
        {
            var properties = new Properties();

            properties = Common(OneOnly(properties, portSeed), 1, applicationClassname);

            var clusterProperties = Properties.OpenWith(properties);

            return clusterProperties;
        }

        private static Properties OneOnly(Properties properties, AtomicInteger portSeed) => AllOf(properties, 1, portSeed);

        private static Properties AllOf(Properties properties, int totalNodes, AtomicInteger portSeed)
        {
            var build = new StringBuilder();

            for (var idx = 1; idx <= totalNodes; ++idx)
            {
                var node = $"node{idx}";

                if (idx > 1)
                {
                    build.Append(",");
                }

                build.Append(node);

                var nodePropertyName = $"node.{node}";

                properties.SetProperty($"{nodePropertyName}.id", $"{idx}");
                properties.SetProperty($"{nodePropertyName}.name", node);
                properties.SetProperty($"{nodePropertyName}.host", "localhost");
                properties.SetProperty($"{nodePropertyName}.op.port", NextPortToUseString(portSeed));
                properties.SetProperty($"{nodePropertyName}.app.port", NextPortToUseString(portSeed));
            }

            properties.SetProperty("cluster.seedNodes", build.ToString());

            return properties;
        }
        
        private static Properties Common(Properties properties, int totalNodes, string applicationClassname)
        {
            properties.SetProperty("cluster.ssl", "false");

            properties.SetProperty("cluster.op.buffer.size", "4096");
            properties.SetProperty("cluster.app.buffer.size", "10240");
            properties.SetProperty("cluster.op.outgoing.pooled.buffers", "20");
            properties.SetProperty("cluster.app.outgoing.pooled.buffers", "50");

            properties.SetProperty("cluster.msg.charset", "UTF-8");

            properties.SetProperty("cluster.app.class", applicationClassname);
            properties.SetProperty("cluster.app.stage", "fake.app.stage");

            properties.SetProperty("cluster.health.check.interval", "2000");
            properties.SetProperty("cluster.live.node.timeout", "20000");
            properties.SetProperty("cluster.heartbeat.interval", "7000");
            properties.SetProperty("cluster.quorum.timeout", "60000");

            return properties;
        }

        private static int NextPortToUse(AtomicInteger portSeed) => portSeed.IncrementAndGet();

        private static string NextPortToUseString(AtomicInteger portSeed) => $"{NextPortToUse(portSeed)}";
    }
}