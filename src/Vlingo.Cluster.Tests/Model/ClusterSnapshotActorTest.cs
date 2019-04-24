// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.IO;
using Vlingo.Actors;
using Vlingo.Cluster.Model;
using Vlingo.Cluster.Model.Message;
using Vlingo.Cluster.Model.Node;
using Vlingo.Wire.Fdx.Inbound;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model
{
    public class ClusterSnapshotActorTest : AbstractClusterTest
    {
        private RawMessage _opMessage;
        private ClusterSnapshotInitializer _intializer;

        [Fact]
        public void TestClusterSnapshot()
        {
            var snapshot =
                TestWorld.ActorFor<IClusterSnapshot>(
                    Definition.Has<ClusterSnapshotActor>(Definition.Parameters(_intializer, Application)));
            
            snapshot.Actor.QuorumAchieved();
            Assert.Equal(1, Application.InformQuorumAchievedCheck.Get());
    
            snapshot.Actor.QuorumLost();
            Assert.Equal(1, Application.InformQuorumLostCheck.Get());
        }

        [Fact]
        public void TestClusterSnapshotControl()
        {
            var control =
                TestWorld.ActorFor<IClusterSnapshotControl>(
                    Definition.Has<ClusterSnapshotActor>(Definition.Parameters(_intializer, Application)));
            
            control.Actor.ShutDown();
            Assert.Equal(1, Application.StopCheck.Get());
        }

        [Fact]
        public void TestInboundStreamInterest()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    Definition.Has<ClusterSnapshotActor>(Definition.Parameters(_intializer, Application)));
            
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, _opMessage);
            Assert.Equal(0, Application.HandleApplicationMessageCheck.Get());
    
            var appMessage = RawMessage.From(1, 0, "app-test");
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.App, appMessage);
            Assert.Equal(1, Application.HandleApplicationMessageCheck.Get());
        }

        [Fact]
        public void TestRegistryInterest()
        {
            var registryInterest =
                TestWorld.ActorFor<IRegistryInterest>(
                    Definition.Has<ClusterSnapshotActor>(Definition.Parameters(_intializer, Application)));
            
            registryInterest.Actor.InformAllLiveNodes(Config.AllNodes, true);
            Assert.Equal(1, Application.AllLiveNodes.Get());
    
            registryInterest.Actor.InformConfirmedByLeader(Config.NodeMatching(Id.Of(1)), true);
            Assert.Equal(1, Application.InformNodeIsHealthyCheck.Get());
    
            registryInterest.Actor.InformCurrentLeader(Config.NodeMatching(Id.Of(3)), true);
            Assert.Equal(1, Application.InformLeaderElectedCheck.Get());
    
            var nodes = new List<Vlingo.Wire.Node.Node>();
            nodes.Add(Config.NodeMatching(Id.Of(3)));
            var mergeResult = new List<MergeResult>();
            mergeResult.Add(new MergeResult(Config.NodeMatching(Id.Of(2)), true));
            mergeResult.Add(new MergeResult(Config.NodeMatching(Id.Of(1)), false));
            registryInterest.Actor.InformMergedAllDirectoryEntries(nodes, mergeResult, true);
            Assert.Equal(1, Application.InformNodeJoinedClusterCheck.Get());
            Assert.Equal(1, Application.InformNodeLeftClusterCheck.Get());
    
            registryInterest.Actor.InformLeaderDemoted(Config.NodeMatching(Id.Of(2)), true);
            Assert.Equal(1, Application.InformLeaderLostCheck.Get());
    
            registryInterest.Actor.InformNodeIsHealthy(Config.NodeMatching(Id.Of(2)), true);
            Assert.Equal(2, Application.InformNodeIsHealthyCheck.Get());
    
            registryInterest.Actor.InformNodeJoinedCluster(Config.NodeMatching(Id.Of(2)), true);
            Assert.Equal(2, Application.InformNodeJoinedClusterCheck.Get());
    
            registryInterest.Actor.InformNodeLeftCluster(Config.NodeMatching(Id.Of(2)), true);
            Assert.Equal(2, Application.InformNodeLeftClusterCheck.Get());
    
            registryInterest.Actor.InformNodeTimedOut(Config.NodeMatching(Id.Of(2)), true);
            Assert.Equal(3, Application.InformNodeLeftClusterCheck.Get());
        }

        public ClusterSnapshotActorTest(ITestOutputHelper output) : base(output)
        {
            _intializer = new ClusterSnapshotInitializer("node1", Properties, TestWorld.DefaultLogger);
    
            var messageBuffer = new MemoryStream(4096);
            var pulse = new Pulse(Id.Of(1));
            MessageConverters.MessageToBytes(pulse, messageBuffer);
            _opMessage = Id.Of(1).Value.ToRawMessage(messageBuffer);
        }
    }
}