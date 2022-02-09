// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections;
using Vlingo.Xoom.Common;
using Xunit;

namespace Vlingo.Xoom.Cluster.Tests
{
    public class ClusterPropertiesTest
    {
        [Fact]
        public void ShouldConfigureMultiNodeCluster() {
          var properties = ClusterProperties.AllNodes();

          // common
          Assert.Equal(4096, properties.GetInteger("cluster.op.buffer.size", 0));
          Assert.Equal(10240, properties.GetInteger("cluster.app.buffer.size", 0));

          var seedNodes = properties.GetString("cluster.seedNodes", "").Split(',');

          Assert.Equal(3, seedNodes.Length);
          Assert.Equal("node1", seedNodes[0]);
          Assert.Equal("node2", seedNodes[1]);
          Assert.Equal("node3", seedNodes[2]);

          // node specific
          Assert.Equal("1", properties.GetString("node.node1.id", ""));
          Assert.Equal("node1", properties.GetString("node.node1.name", ""));
          Assert.Equal("localhost", properties.GetString("node.node1.host", ""));

          Assert.Equal("2", properties.GetString("node.node2.id", ""));
          Assert.Equal("node2", properties.GetString("node.node2.name", ""));
          Assert.Equal("localhost", properties.GetString("node.node2.host", ""));

          Assert.Equal("3", properties.GetString("node.node3.id", ""));
          Assert.Equal("node3", properties.GetString("node.node3.name", ""));
          Assert.Equal("localhost", properties.GetString("node.node3.host", ""));
        }

        [Fact]
        public void ShouldConfigureFiveNodeCluster() {
          var properties = ClusterProperties.AllNodes(new AtomicInteger(37370), 5);

          // common
          Assert.Equal(4096, properties.GetInteger("cluster.op.buffer.size", 0));
          Assert.Equal(10240, properties.GetInteger("cluster.app.buffer.size", 0));

          var seedNodes = properties.GetString("cluster.seedNodes", "").Split(',');

          Assert.Equal(5, seedNodes.Length);
          Assert.Equal("node1", seedNodes[0]);
          Assert.Equal("node2", seedNodes[1]);
          Assert.Equal("node3", seedNodes[2]);
          Assert.Equal("node4", seedNodes[3]);
          Assert.Equal("node5", seedNodes[4]);

          // node specific
          Assert.Equal("1", properties.GetString("node.node1.id", ""));
          Assert.Equal("node1", properties.GetString("node.node1.name", ""));
          Assert.Equal("localhost", properties.GetString("node.node1.host", ""));

          Assert.Equal("2", properties.GetString("node.node2.id", ""));
          Assert.Equal("node2", properties.GetString("node.node2.name", ""));
          Assert.Equal("localhost", properties.GetString("node.node2.host", ""));

          Assert.Equal("3", properties.GetString("node.node3.id", ""));
          Assert.Equal("node3", properties.GetString("node.node3.name", ""));
          Assert.Equal("localhost", properties.GetString("node.node3.host", ""));

          Assert.Equal("4", properties.GetString("node.node4.id", ""));
          Assert.Equal("node4", properties.GetString("node.node4.name", ""));
          Assert.Equal("localhost", properties.GetString("node.node3.host", ""));

          Assert.Equal("5", properties.GetString("node.node5.id", ""));
          Assert.Equal("node5", properties.GetString("node.node5.name", ""));
          Assert.Equal("localhost", properties.GetString("node.node5.host", ""));
        }

        [Fact]
        public void ShouldConfigureSingleNodeCluster() {
          var properties = ClusterProperties.OneNode();

          // common
          Assert.Equal(4096, properties.GetInteger("cluster.op.buffer.size", 0));
          Assert.Equal(10240, properties.GetInteger("cluster.app.buffer.size", 0));

          var seedNodes = properties.GetString("cluster.seedNodes", "").Split(',');

          Assert.Single((IEnumerable) seedNodes);
          Assert.Equal("node1", seedNodes[0]);

          // node specific
          Assert.Equal("1", properties.GetString("node.node1.id", ""));
          Assert.Equal("node1", properties.GetString("node.node1.name", ""));
          Assert.Equal("localhost", properties.GetString("node.node1.host", ""));
        }
    }
}