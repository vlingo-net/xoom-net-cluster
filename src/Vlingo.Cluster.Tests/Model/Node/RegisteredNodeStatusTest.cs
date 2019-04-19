// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model.Node;
using Xunit;

namespace Vlingo.Cluster.Tests.Model.Node
{
    using Vlingo.Wire.Node;
    
    public class RegisteredNodeStatusTest
    {
        [Fact]
        public void TestStatusCreationState()
        {
            var id1 = Id.Of(1);
            var name1 = new Name("name1");
            var opAddress1 = new Address(Host.Of("localhost"), 11111, AddressType.Op);
            var appAddress1 = new Address(Host.Of("localhost"), 11112, AddressType.App);
            var node1 = new Node(id1, name1, opAddress1, appAddress1);
            
            var status = new RegisteredNodeStatus(node1, true, true);
            
            Assert.True(status.IsLeader);
            Assert.True(status.IsConfirmedByLeader);
            status.ConfirmedByLeader(false);
            Assert.False(status.IsConfirmedByLeader);
            Assert.Equal(node1, status.Node);
            Assert.True(status.LastHealthIndication > 0L);
        }

        [Fact]
        public void TestStatusTimeout()
        {
            var id1 = Id.Of(1);
            var name1 = new Name("name1");
            var opAddress1 = new Address(Host.Of("localhost"), 11111, AddressType.Op);
            var appAddress1 = new Address(Host.Of("localhost"), 11112, AddressType.App);
            var node1 = new Node(id1, name1, opAddress1, appAddress1);

            var status = new RegisteredNodeStatus(node1, true, true);
    
            Assert.False(status.IsTimedOut(RegisteredNodeStatus.CurrentTimeMillis(), 100L));
            Assert.True(status.IsTimedOut(RegisteredNodeStatus.CurrentTimeMillis() + 4001L, 4000L));
        }
    }
}