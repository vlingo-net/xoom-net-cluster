// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model;
using Vlingo.Xoom.Wire.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model
{
    public class ClusterApplicationBroadcasterTest : AbstractClusterTest
    {
        private readonly ClusterApplicationBroadcaster _broadcaster;

        [Fact]
        public void TestInformAllLiveNodes()
        {
            _broadcaster.InformAllLiveNodes(Config.AllNodes, true);
            Assert.Equal(1, Application.AllLiveNodes.Get());
        }
        
        [Fact]
        public void TestInformLeaderElected()
        {
            _broadcaster.InformLeaderElected(Id.Of(3), true, false);
            Assert.Equal(1, Application.InformLeaderElectedCheck.Get());
        }
        
        [Fact]
        public void TestInformLeaderLost()
        {
            _broadcaster.InformLeaderLost(Id.Of(3), false);
            Assert.Equal(1, Application.InformLeaderLostCheck.Get());
        }
        
        [Fact]
        public void TestInformLocalNodeShutDown()
        {
            _broadcaster.InformLocalNodeShutDown(Id.Of(1));
            Assert.Equal(1, Application.InformLocalNodeShutDownCheck.Get());
        }
        
        [Fact]
        public void TestInformLocalNodeStarted()
        {
            _broadcaster.InformLocalNodeStarted(Id.Of(1));
            Assert.Equal(1, Application.InformLocalNodeStartedCheck.Get());
        }
        
        [Fact]
        public void TestInformNodeIsHealthy()
        {
            _broadcaster.InformNodeIsHealthy(Id.Of(1), true);
            Assert.Equal(1, Application.InformNodeIsHealthyCheck.Get());
        }
        
        [Fact]
        public void TestInformNodeJoinedCluster()
        {
            _broadcaster.InformNodeJoinedCluster(Id.Of(2), true);
            Assert.Equal(1, Application.InformNodeJoinedClusterCheck.Get());
        }
        
        [Fact]
        public void TestInformNodeLeftCluster()
        {
            _broadcaster.InformNodeLeftCluster(Id.Of(2), true);
            Assert.Equal(1, Application.InformNodeLeftClusterCheck.Get());
        }
        
        [Fact]
        public void TestInformQuorumAchieved()
        {
            _broadcaster.InformQuorumAchieved();
            Assert.Equal(1, Application.InformQuorumAchievedCheck.Get());
        }
        
        [Fact]
        public void TestInformQuorumLost()
        {
            _broadcaster.InformQuorumLost();
            Assert.Equal(1, Application.InformQuorumLostCheck.Get());
        }
        
        [Fact]
        public void TestInformResponder()
        {
            _broadcaster.InformResponder(null); // production must not be null
            Assert.Equal(1, Application.InformResponderCheck.Get());
        }
        
        [Fact]
        public void TestInformAttributeSetCreated()
        {
            _broadcaster.InformAttributeSetCreated("test");
            Assert.Equal(1, Application.InformAttributeSetCreatedCheck.Get());
        }
        
        [Fact]
        public void TestInformAttributeAdded()
        {
            _broadcaster.InformAttributeAdded("test", "test");
            Assert.Equal(1, Application.InformAttributeAddedCheck.Get());
        }
        
        [Fact]
        public void TestInformAttributeRemoved()
        {
            _broadcaster.InformAttributeRemoved("test", "test");
            Assert.Equal(1, Application.InformAttributeRemovedCheck.Get());
        }
        
        [Fact]
        public void TestInformAttributeReplaced()
        {
            _broadcaster.InformAttributeReplaced("test", "test");
            Assert.Equal(1, Application.InformAttributeReplacedCheck.Get());
        }
        
        public ClusterApplicationBroadcasterTest(ITestOutputHelper output) : base(output)
        {
            _broadcaster = new ClusterApplicationBroadcaster(TestWorld.DefaultLogger);
            _broadcaster.RegisterClusterApplication(Application);
        }
    }
}