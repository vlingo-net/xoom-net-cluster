// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Wire.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model
{
    public class ClusterConfigurationTest : AbstractClusterTest
    {
        [Fact]
        public void TestAllConfiguredNodes()
        {
            var all = Config.AllNodes.ToList();
            
            Assert.Equal(3, all.Count);

            var nodes = new List<Node>();
            nodes.Add(NextNodeWith(1));
            nodes.Add(NextNodeWith(2));
            nodes.Add(NextNodeWith(3));

            foreach (var node in all)
            {
                nodes.Remove(node);
            }
            
            Assert.Empty(nodes);
        }
        
        [Fact]
        public void TestAllConfiguredNodeNames()
        {
            var all = Config.AllNodeNames.ToList();
            
            Assert.Equal(3, all.Count);

            var allNames = new List<string>();
            allNames.Add("node1");
            allNames.Add("node2");
            allNames.Add("node3");

            foreach (var nodeName in all)
            {
                allNames.Remove(nodeName);
            }
            
            Assert.Empty(allNames);
        }

        [Fact]
        public void TestAllOtherConfiguredNodes()
        {
            Assert.Equal(2, Config.AllOtherNodes(Id.Of(1)).Count());
            Assert.Equal(2, Config.AllOtherNodes(Id.Of(2)).Count());
            Assert.Equal(2, Config.AllOtherNodes(Id.Of(3)).Count());
            Assert.Equal(3, Config.AllOtherNodes(Id.Of(4)).Count());
        }
        
        [Fact]
        public void TestAllGreaterConfiguredNodes()
        {
            Assert.Equal(3, Config.AllGreaterNodes(Id.Of(0)).Count());
            Assert.Equal(2, Config.AllGreaterNodes(Id.Of(1)).Count());
            Assert.Single(Config.AllGreaterNodes(Id.Of(2)));
            Assert.Empty(Config.AllGreaterNodes(Id.Of(3)));
        }
        
        [Fact]
        public void TestConfiguredNodeMatching()
        {
            Assert.Equal(Node.NoNode, Config.NodeMatching(Id.Of(0)));
            Assert.Equal("node1", Config.NodeMatching(Id.Of(1)).Name.Value);
            Assert.Equal("node2", Config.NodeMatching(Id.Of(2)).Name.Value);
            Assert.Equal("node3", Config.NodeMatching(Id.Of(3)).Name.Value);
            Assert.Equal(Node.NoNode, Config.NodeMatching(Id.Of(4)));
        }

        [Fact]
        public void TestGreatestConfiguredNodeId()
        {
            Assert.Equal(Id.Of(3), Config.GreatestNodeId);
        }

        [Fact]
        public void TestHasConfiguredNode()
        {
            Assert.False(Config.HasNode(Id.Of(0)));
            Assert.True(Config.HasNode(Id.Of(1)));
            Assert.True(Config.HasNode(Id.Of(2)));
            Assert.True(Config.HasNode(Id.Of(3)));
            Assert.False(Config.HasNode(Id.Of(4)));
        }
        
        [Fact]
        public void TestTotalConfiguredNodes()
        {
            Assert.Equal(3, Config.TotalNodes);
        }

        public ClusterConfigurationTest(ITestOutputHelper output) : base(output)
        {
        }
    }
}