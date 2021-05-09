// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Xoom.Cluster.Tests.Model
{
    public class ClusterTest : AbstractClusterTest
    {
        [Fact]
        public void TestClusterSnapshotControl()
        {
            var (control, _) = Vlingo.Xoom.Cluster.Model.Cluster.ControlFor("node1");

            Assert.NotNull(control);
        }
        
        public ClusterTest(ITestOutputHelper output) : base(output)
        {
        }
    }
}