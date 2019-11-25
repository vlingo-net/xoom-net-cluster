// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model
{
    public class ClusterTest : AbstractClusterTest
    {
        private static int _count = 0;

        [Fact]
        public void TestClusterSnapshotControl()
        {
            var (control, logger) = Vlingo.Cluster.Model.Cluster.ControlFor("node1");

            Assert.NotNull(control);

            ++_count;
            logger.Debug($"======== ClusterTest#testClusterSnapshotControl({_count}) ========");

            Assert.True(Vlingo.Cluster.Model.Cluster.IsRunning(true, 10));

            control.ShutDown();

            Assert.False(Vlingo.Cluster.Model.Cluster.IsRunning(false, 10));
        }
        
        public ClusterTest(ITestOutputHelper output) : base(output)
        {
        }
    }
}