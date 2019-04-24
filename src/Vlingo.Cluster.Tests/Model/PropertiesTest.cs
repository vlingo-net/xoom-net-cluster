// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model
{
    public class PropertiesTest : AbstractClusterTest
    {
        [Fact]
        public void TestApplicationBufferSize()
        {
            Assert.Equal(10240, Properties.ApplicationBufferSize());
        }

        [Fact]
        public void TestApplicationOutgoingPooledBuffers()
        {
            Assert.Equal(50, Properties.ApplicationOutgoingPooledBuffers());
        }
        
        [Fact]
        public void TestClusterApplicationType()
        {
            Assert.NotNull(Properties.ClusterApplicationTypeName());
            Assert.NotNull(Properties.ClusterApplicationType());
        }
        
        [Fact]
        public void TestClusterHealthCheckInterval()
        {
            Assert.Equal(2000, Properties.ClusterHealthCheckInterval());
        }
        
        [Fact]
        public void TestClusterHeartbeatInterval()
        {
            Assert.Equal(7000, Properties.ClusterHeartbeatInterval());
        }
        
        [Fact]
        public void TestClusterLiveNodeTimeout()
        {
            Assert.Equal(20000, Properties.ClusterLiveNodeTimeout());
        }
        
        [Fact]
        public void TestClusterQuorumTimeout()
        {
            Assert.Equal(60000, Properties.ClusterQuorumTimeout());
        }
        
        [Fact]
        public void TestOperationalBufferSize()
        {
            Assert.Equal(4096, Properties.OperationalBufferSize());
        }
        
        [Fact]
        public void TestOperationalOutgoingPooledBuffers()
        {
            Assert.Equal(20, Properties.OperationalOutgoingPooledBuffers());
        }
        
        [Fact]
        public void TestSeedNodes()
        {
            var seedNodes = Properties.SeedNodes().ToList();
            Assert.Equal(3, seedNodes.Count);
            Assert.Contains("node1", seedNodes);
            Assert.Contains("node1", seedNodes);
            Assert.Contains("node1", seedNodes);
        }
        
        [Fact]
        public void TestUseSSL()
        {
            Assert.False(Properties.UseSSL());
        }
        
        [Fact]
        public void TestNodes1()
        {
            Assert.Equal(1, Properties.NodeId("node1"));
            Assert.Equal("node1", Properties.NodeName("node1"));
            Assert.Equal("localhost", Properties.Host("node1"));
        }
        
        [Fact]
        public void TestNodes2()
        {
            Assert.Equal(2, Properties.NodeId("node2"));
            Assert.Equal("node2", Properties.NodeName("node2"));
            Assert.Equal("localhost", Properties.Host("node2"));
        }
        
        [Fact]
        public void TestNodes3()
        {
            Assert.Equal(3, Properties.NodeId("node3"));
            Assert.Equal("node3", Properties.NodeName("node3"));
            Assert.Equal("localhost", Properties.Host("node3"));
        }
        
        public PropertiesTest(ITestOutputHelper output) : base(output)
        {
        }
    }
}