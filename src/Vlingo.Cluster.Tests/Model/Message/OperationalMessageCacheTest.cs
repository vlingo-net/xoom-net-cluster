// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model.Message;
using Xunit;

namespace Vlingo.Cluster.Tests.Model.Message
{
    using Vlingo.Wire.Node;
    
    public class OperationalMessageCacheTest
    {
        [Fact]
        public void TestCachedMessages()
        {
            var node1 = Node.With(Id.Of(2), Name.Of("node2"), Host.Of("localhost"), 37373, 37374);
            var cache = new OperationalMessageCache(node1);
            
            var elect = cache.CachedRawMessage(OperationalMessage.ELECT);
            Assert.True(OperationalMessage.MessageFrom(elect.AsTextMessage()).IsElect);
            
            var join = cache.CachedRawMessage(OperationalMessage.JOIN);
            Assert.True(OperationalMessage.MessageFrom(join.AsTextMessage()).IsJoin);
            
            var leader = cache.CachedRawMessage(OperationalMessage.LEADER);
            Assert.True(OperationalMessage.MessageFrom(leader.AsTextMessage()).IsLeader);
            
            var leave = cache.CachedRawMessage(OperationalMessage.LEAVE);
            Assert.True(OperationalMessage.MessageFrom(leave.AsTextMessage()).IsLeave);
            
            var ping = cache.CachedRawMessage(OperationalMessage.PING);
            Assert.True(OperationalMessage.MessageFrom(ping.AsTextMessage()).IsPing);
            
            var pulse = cache.CachedRawMessage(OperationalMessage.PULSE);
            Assert.True(OperationalMessage.MessageFrom(pulse.AsTextMessage()).IsPulse);
            
            var vote = cache.CachedRawMessage(OperationalMessage.VOTE);
            Assert.True(OperationalMessage.MessageFrom(vote.AsTextMessage()).IsVote);
        }

        [Fact]
        public void TestNonCachedMessages()
        {
            var node1 = Node.With(Id.Of(2), Name.Of("node2"), Host.Of("localhost"), 37373, 37374);
            var cache = new OperationalMessageCache(node1);
            
            bool caught;
    
            try
            {
                caught = false;
                cache.CachedRawMessage(OperationalMessage.CHECKHEALTH);
            } 
            catch
            {
                caught = true;
            }
            Assert.True(caught);
    
            try
            {
                caught = false;
                cache.CachedRawMessage(OperationalMessage.DIR);
            } 
            catch
            {
                caught = true;
            }
            Assert.True(caught);
    
            try
            {
                caught = false;
                cache.CachedRawMessage(OperationalMessage.SPLIT);
            }
            catch
            {
                caught = true;
            }
            Assert.True(caught);
        }
    }
}