// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Cluster.Model.Nodes;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Wire.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Xoom.Cluster.Tests.Model.Nodes
{
    public class LocalRegistryTest : AbstractClusterTest
    {
        [Fact]
        public void TestNoLiveNodes()
        {
            var registry = new LocalRegistry(Config.NodeMatching(Id.Of(3)), Config, TestWorld.DefaultLogger);
            Assert.Empty(registry.LiveNodes);
        }

        [Fact]
        public void TestCleanTimedOutNodes()
        {
            var registry = Join3Nodes();
    
            Assert.Equal(3, registry.LiveNodes.Count());
    
            registry.UpdateLastHealthIndication(Id.Of(1));
    
            registry.RegisteredNodeStatusOf(Id.Of(1)).SetLastHealthIndication(DateTimeHelper.CurrentTimeMillis() - 70001);
    
            registry.CleanTimedOutNodes();
    
            Assert.Equal(2, registry.LiveNodes.Count());
        }
        
        [Fact]
        public void TestConfirmAllLiveNodesByLeader()
        {
            var registry = Join3Nodes();
    
            registry.ConfirmAllLiveNodesByLeader();

            foreach (var node in registry.LiveNodes)
            {
                Assert.True(registry.IsConfirmedByLeader(node.Id));
            }
            
            Assert.False(registry.IsConfirmedByLeader(IdOf(4)));
        }

        [Fact]
        public void TestDeclareLeaderHasLeader()
        {
            var registry = Join3Nodes();
            
            var id1 = IdOf(1);
            var id2 = IdOf(2);
            var id3 = IdOf(3);
    
            registry.DeclareLeaderAs(id3);
    
            Assert.True(registry.HasLeader);
    
            Assert.False(registry.IsLeader(id1));
            Assert.False(registry.IsLeader(id2));
            Assert.True(registry.IsLeader(id3));
            Assert.False(registry.IsLeader(IdOf(4)));
        }

        [Fact]
        public void TestDemoteLeaderHasLeader()
        {
            var registry = Join3Nodes();
            
            var id3 = IdOf(3);
            
            registry.DeclareLeaderAs(id3);
    
            Assert.True(registry.HasLeader);
            Assert.True(registry.IsLeader(id3));
    
            registry.DemoteLeaderOf(id3);
    
            Assert.False(registry.HasLeader);
            Assert.False(registry.IsLeader(id3));
        }

        [Fact]
        public void TestHasLeader()
        {
            var registry = Join3Nodes();
            
            var id3 = IdOf(3);
            
            registry.DeclareLeaderAs(id3);
            Assert.True(registry.HasLeader);

            registry.DemoteLeaderOf(id3);
            Assert.False(registry.HasLeader);
        }

        [Fact]
        public void TestIsLeader()
        {
            var registry = Join3Nodes();
            
            var id3 = IdOf(3);
            
            registry.DeclareLeaderAs(id3);
            Assert.True(registry.IsLeader(id3));
            Assert.False(registry.IsLeader(IdOf(2)));
        }

        [Fact]
        public void TestHasMember()
        {
            var registry = Join3Nodes();
    
            Assert.True(registry.HasMember(IdOf(1)));
            Assert.True(registry.HasMember(IdOf(1)));
            Assert.True(registry.HasMember(IdOf(1)));
            Assert.False(registry.HasMember(IdOf(4)));
        }

        [Fact]
        public void TestHasQuorum()
        {
            var registry = Join3Nodes();
            Assert.True(registry.HasQuorum);
    
            registry.Leave(IdOf(2));
            Assert.True(registry.HasQuorum);
    
            registry.Leave(IdOf(3));
            Assert.False(registry.HasQuorum);
        }

        [Fact]
        public void TestJoin()
        {
            var registry = Join3Nodes();
            
            registry.Join(NodeOf(4));
    
            Assert.Equal(4, registry.LiveNodes.Count());
    
            Assert.True(registry.HasMember(IdOf(1)));
            Assert.True(registry.HasMember(IdOf(2)));
            Assert.True(registry.HasMember(IdOf(3)));
            Assert.True(registry.HasMember(IdOf(4)));
        }

        [Fact]
        public void TestLeave()
        {
            var registry = Join3Nodes();
            
            Assert.True(registry.HasMember(IdOf(1)));
            Assert.True(registry.HasMember(IdOf(1)));
            Assert.True(registry.HasMember(IdOf(1)));
    
            registry.Leave(IdOf(2));
    
            Assert.True(registry.HasMember(IdOf(1)));
            Assert.False(registry.HasMember(IdOf(2)));
            Assert.True(registry.HasMember(IdOf(3)));
        }

        [Fact]
        public void TestMergeAllDirectoryEntries()
        {
            var registry = Join3Nodes();
            
            var leaderRegisteredNodesToMerge = new List<Node>();
    
            leaderRegisteredNodesToMerge.Add(NodeOf(1));
            leaderRegisteredNodesToMerge.Add(NodeOf(2));
            leaderRegisteredNodesToMerge.Add(NodeOf(4));
    
            var interest = new MockRegistryInterest();
    
            registry.RegisterRegistryInterest(interest);
    
            registry.MergeAllDirectoryEntries(leaderRegisteredNodesToMerge);
    
            Assert.Equal(1, interest.InformMergedAllDirectoryEntriesCheck);
            Assert.Equal(3, interest.LiveNodes.Count());  // 1, 2, 4
            Assert.Equal(2, interest.MergeResults.Count()); // 4 joined, 3 left
    
            var list = interest.MergeResults.ToList();
    
            var inspectable1 = list[0];
            var inspectable2 = list[1];
    
            Assert.Equal(IdOf(3), inspectable1.Node.Id);
            Assert.True(inspectable1.Left);
    
            Assert.Equal(IdOf(4), inspectable2.Node.Id);
            Assert.True(inspectable2.Joined);
        }

        [Fact]
        public void TestPromoteElectedLeader()
        {
            var registry = Join3Nodes();
            
            var interest = new MockRegistryInterest();
    
            registry.RegisterRegistryInterest(interest);

            registry.Join(NodeOf(4));
    
            registry.DeclareLeaderAs(IdOf(3));
    
            Assert.Equal(1, interest.InformCurrentLeaderCheck);
    
            registry.PromoteElectedLeader(IdOf(4));
    
            Assert.Equal(1, interest.InformLeaderDemotedCheck);
            Assert.Equal(3, interest.InformCurrentLeaderCheck);
            Assert.Equal(4, registry.CurrentLeader.Id.Value);
        }

        [Fact]
        public void TestUpdateLastHealthIndication()
        {
            var registry = Join3Nodes();
            var minTime = DateTimeHelper.CurrentTimeMillis();
            var maxTime = minTime + 100L;

            foreach (var node in registry.LiveNodes)
            {
                registry.UpdateLastHealthIndication(node.Id);
            }

            foreach (var healthyNode in registry.LiveNodes)
            {
                var status = registry.RegisteredNodeStatusOf(healthyNode.Id);
                var lastHealthIndication = status.LastHealthIndication;
                Assert.True(lastHealthIndication >= minTime && lastHealthIndication <= maxTime);
            }
        }

        public LocalRegistryTest(ITestOutputHelper output) : base(output)
        {
        }
        
        private Id IdOf(int idValue)
        {
            return Id.Of(idValue);
        }
  
        private LocalRegistry Join3Nodes()
        {
            var registry = new LocalRegistry(Config.NodeMatching(Id.Of(3)), Config, TestWorld.DefaultLogger);
            var node1 = NodeOf(1);
            var node2 = NodeOf(2);
            var node3 = NodeOf(3);

            registry.Join(node1);
            registry.Join(node2);
            registry.Join(node3);
    
            return registry;
        }
  
        private Node NodeOf(int idValue)
        {
            var id = Id.Of(idValue);
            var name = new Name($"node{idValue}");
            var opAddress = new Address(Host.Of("localhost"), 1111 + idValue, AddressType.Op);
            var appAddress = new Address(Host.Of("localhost"), 1111 + idValue+1, AddressType.App);
            var node = new Node(id, name, false, opAddress, appAddress);
    
            return node;
        }
    }
}