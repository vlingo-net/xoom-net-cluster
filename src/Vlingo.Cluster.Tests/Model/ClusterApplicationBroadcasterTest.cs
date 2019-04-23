// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model
{
    public class ClusterApplicationBroadcasterTest : AbstractClusterTest
    {
        private ClusterApplicationBroadcaster _broadcaster;

        [Fact]
        public void TestInformAllLiveNodes()
        {
            _broadcaster.InformAllLiveNodes(Config.AllNodes, true);
            Assert.Equal(1, Application.AllLiveNodes.Get());
        }
        
        public ClusterApplicationBroadcasterTest(ITestOutputHelper output) : base(output)
        {
            _broadcaster = new ClusterApplicationBroadcaster(TestWorld.DefaultLogger);
            _broadcaster.RegisterClusterApplication(Application);
        }
    }
}