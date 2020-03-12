// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Cluster.Model.Message;
using Xunit;

namespace Vlingo.Cluster.Tests.Model.Message
{
    using Vlingo.Wire.Node;
    
    public class OperationalMessageParserTest
    {
        [Fact]
        public void TestParseDirectory()
        {
            var dir = OperationalMessage.MessageFrom(MessageFixtures.DirectoryAsText(1, 2, 3));
    
            Assert.True(dir.IsDirectory);
            Assert.Equal(Id.Of(MessageFixtures.DefaultNodeId), dir.Id);
            Assert.Equal(new Name(MessageFixtures.DefaultNodeName), ((Directory) dir).Name);

            int index = 1;
            foreach (var node in ((Directory) dir).Nodes)
            {
                Assert.Equal(Id.Of(index), node.Id);
                Assert.Equal(new Name("node" + index), node.Name);
                Assert.Equal(Address.From(MessageFixtures.OpAddresses[index], AddressType.Op), node.OperationalAddress);
                Assert.Equal(Address.From(MessageFixtures.AppAddresses[index], AddressType.App), node.ApplicationAddress);

                ++index;
            }

            var nodeEntries = new HashSet<Node>();
            nodeEntries.Add(new Node(
                Id.Of(1),
                new Name("node1"),
                Address.From(MessageFixtures.OpAddresses[1], AddressType.Op),
                Address.From(MessageFixtures.AppAddresses[1], AddressType.App)));
            nodeEntries.Add(new Node(
                Id.Of(2),
                new Name("node2"),
                Address.From(MessageFixtures.OpAddresses[2], AddressType.Op),
                Address.From(MessageFixtures.AppAddresses[2], AddressType.App)));
            nodeEntries.Add(new Node(
                Id.Of(3),
                new Name("node3"),
                Address.From(MessageFixtures.OpAddresses[3], AddressType.Op),
                Address.From(MessageFixtures.AppAddresses[3], AddressType.App)));

            var expectedDir = new Directory(Id.Of(MessageFixtures.DefaultNodeId), new Name(MessageFixtures.DefaultNodeName), nodeEntries);

            Assert.Equal(expectedDir, dir);
        }

        [Fact]
        public void TestParseElect()
        {
            var elect1 = OperationalMessage.MessageFrom(OperationalMessage.ELECT + "\n" + "id=1");
            Assert.True(elect1.IsElect);
            Assert.Equal(Id.Of(1), elect1.Id);
            var expectedElec1 = new Elect(Id.Of(1));
            Assert.Equal(expectedElec1, elect1);

            var elect100 = OperationalMessage.MessageFrom(OperationalMessage.ELECT + "\n" + "id=100");
            Assert.True(elect100.IsElect);
            Assert.Equal(Id.Of(100), elect100.Id);
            var expectedElec100 = new Elect(Id.Of(100));
            Assert.Equal(expectedElec100, elect100);
        }

        [Fact]
        public void TestParseJoin()
        {
            var join = OperationalMessage.MessageFrom(MessageFixtures.JoinAsText());
            Assert.True(join.IsJoin);
            var expectedNode =
                new Node(
                    Id.Of(1),
                    new Name("node1"),
                    Address.From("localhost:37371", AddressType.Op),
                    Address.From("localhost:37372", AddressType.App));
            var expectedJoin = new Join(expectedNode);
            Assert.Equal(expectedNode, ((Join) join).Node);
            Assert.Equal(expectedJoin, join);
        }

        [Fact]
        public void TestParseLeader()
        {
            var leader1 = OperationalMessage.MessageFrom(MessageFixtures.LeaderAsText());
            Assert.True(leader1.IsLeader);
            Assert.Equal(Id.Of(1), leader1.Id);
            var expectedLeader1 = new Leader(Id.Of(1));
            Assert.Equal(expectedLeader1, leader1);

            var leader100 = OperationalMessage.MessageFrom(OperationalMessage.LEADER + "\n" + "id=100");
            Assert.True(leader100.IsLeader);
            Assert.Equal(Id.Of(100), leader100.Id);
            var expectedLeader100 = new Leader(Id.Of(100));
            Assert.Equal(expectedLeader100, leader100);
        }

        [Fact]
        public void TestParseLeave()
        {
            var leave1 = OperationalMessage.MessageFrom(MessageFixtures.LeaveAsText());
            Assert.True(leave1.IsLeave);
            Assert.Equal(Id.Of(1), leave1.Id);
            var expectedLeave1 = new Leave(Id.Of(1));
            Assert.Equal(expectedLeave1, leave1);

            var leave100 = OperationalMessage.MessageFrom(OperationalMessage.LEAVE + "\n" + "id=100");
            Assert.True(leave100.IsLeave);
            Assert.Equal(Id.Of(100), leave100.Id);
            var expectedLeave100 = new Leave(Id.Of(100));
            Assert.Equal(expectedLeave100, leave100);
        }
        
        [Fact]
        public void TestParsePing()
        {
            var ping1 = OperationalMessage.MessageFrom(OperationalMessage.PING + "\n" + "id=1");
            Assert.True(ping1.IsPing);
            Assert.Equal(Id.Of(1), ping1.Id);
            var expectedPing1 = new Ping(Id.Of(1));
            Assert.Equal(expectedPing1, ping1);

            var ping100 = OperationalMessage.MessageFrom(OperationalMessage.PING + "\n" + "id=100");
            Assert.True(ping100.IsPing);
            Assert.Equal(Id.Of(100), ping100.Id);
            var expectedPing100 = new Ping(Id.Of(100));
            Assert.Equal(expectedPing100, ping100);
        }
        
        [Fact]
        public void TestParsePulse()
        {
            var pulse1 = OperationalMessage.MessageFrom(OperationalMessage.PULSE + "\n" + "id=1");
            Assert.True(pulse1.IsPulse);
            Assert.Equal(Id.Of(1), pulse1.Id);
            var expectedPulse1 = new Pulse(Id.Of(1));
            Assert.Equal(expectedPulse1, pulse1);

            var pulse100 = OperationalMessage.MessageFrom(OperationalMessage.PULSE + "\n" + "id=100");
            Assert.True(pulse100.IsPulse);
            Assert.Equal(Id.Of(100), pulse100.Id);
            var expectedPulse100 = new Pulse(Id.Of(100));
            Assert.Equal(expectedPulse100, pulse100);
        }
        
        [Fact]
        public void TestParseVote()
        {
            var vote1 = OperationalMessage.MessageFrom(OperationalMessage.VOTE + "\n" + "id=1");
            Assert.True(vote1.IsVote);
            Assert.Equal(Id.Of(1), vote1.Id);
            var expectedVote1 = new Vote(Id.Of(1));
            Assert.Equal(expectedVote1, vote1);

            var vote100 = OperationalMessage.MessageFrom(OperationalMessage.VOTE + "\n" + "id=100");
            Assert.True(vote100.IsVote);
            Assert.Equal(Id.Of(100), vote100.Id);
            var expectedVote100 = new Vote(Id.Of(100));
            Assert.Equal(expectedVote100, vote100);
        }
    }
}