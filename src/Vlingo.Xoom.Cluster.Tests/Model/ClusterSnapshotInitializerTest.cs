// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Cluster.Model;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Xoom.Cluster.Tests.Model
{
    public class ClusterSnapshotInitializerTest : AbstractClusterTest
    {
        [Fact]
        public void TestCreate()
        {
            var initializer = new ClusterSnapshotInitializer("node1", Properties, TestWorld.DefaultLogger);
            
            Assert.NotNull(initializer);
            Assert.NotNull(initializer.CommunicationsHub);
            Assert.NotNull(initializer.Configuration);
            Assert.NotNull(initializer.LocalNode);
            Assert.Equal(1, initializer.LocalNode.Id.Value);
            Assert.NotNull(initializer.LocalNodeId);
            Assert.Equal(1, initializer.LocalNodeId.Value);
            Assert.NotNull(initializer.Registry);
        }
        
        public ClusterSnapshotInitializerTest(ITestOutputHelper output) : base(output)
        {
        }
    }
}