// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.IO;
using Vlingo.Actors;
using Vlingo.Cluster.Model;
using Vlingo.Cluster.Model.Message;
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