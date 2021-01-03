// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.IO;
using Vlingo.Cluster.Model.Message;
using Vlingo.Wire.Channel;
using Xunit;
using Directory = Vlingo.Cluster.Model.Message.Directory;

namespace Vlingo.Cluster.Tests.Model.Message
{
    using Vlingo.Wire.Message;
    using Vlingo.Wire.Node;
    
    public class OperationalMessageGenerationTest
    {
        private static int _bytes = 4096;
        private MemoryStream _expectedBuffer = new MemoryStream(_bytes);
        private MemoryStream _messageBuffer = new MemoryStream(_bytes);

        [Fact]
        public void TestGenerateApplicationSaidMessage()
        {
            var id = Id.Of(1);
            var name = new Name("node1");
            var payload = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Morbi sagittis risus quis nulla blandit, a euismod massa egestas. Vivamus facilisis.";
    
            var app = ApplicationSays.From(id, name, payload);
            var raw = $"{OperationalMessage.APP}\nid=1 nm=node1 si={app.SaysId}\n{payload}";
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            MessageConverters.MessageToBytes(app, _messageBuffer);
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(app, ApplicationSays.From(raw));
        }

        [Fact]
        public void TestGenerateDirectoryMessge()
        {
            var opAddresses = new [] { "", "localhost:37371", "localhost:37373", "localhost:37375" };
            var appAddresses = new [] { "", "localhost:37372", "localhost:37374", "localhost:37376" };

            var nodeEntries = new List<Node>();
            nodeEntries.Add(new Node(
                Id.Of(1),
                new Name("node1"),
                Address.From(opAddresses[1], AddressType.Op),
                Address.From(appAddresses[1], AddressType.App)));
            nodeEntries.Add(new Node(
                Id.Of(2),
                new Name("node2"),
                Address.From(opAddresses[2], AddressType.Op),
                Address.From(appAddresses[2], AddressType.App)));
            nodeEntries.Add(new Node(
                Id.Of(3),
                new Name("node3"),
                Address.From(opAddresses[3], AddressType.Op),
                Address.From(appAddresses[3], AddressType.App)));
            var dir = new Directory(Id.Of(1), new Name("node1"), nodeEntries);
            MessageConverters.MessageToBytes(dir, _messageBuffer);
            var raw =
                OperationalMessage.DIR + "\n"
                                       + "id=1 nm=node1\n"
                                       + "id=1 nm=node1 op=" + opAddresses[1] + " app=" + appAddresses[1] + "\n"
                                       + "id=2 nm=node2 op=" + opAddresses[2] + " app=" + appAddresses[2] + "\n"
                                       + "id=3 nm=node3 op=" + opAddresses[3] + " app=" + appAddresses[3];
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(dir, Directory.From(raw));
        }

        [Fact]
        public void TestGenerateElectMessage()
        {
            var elect = new Elect(Id.Of(1));
            MessageConverters.MessageToBytes(elect, _messageBuffer);
            var raw = OperationalMessage.ELECT + "\nid=1";
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(elect, Elect.From(raw));
        }

        [Fact]
        public void TestGenerateJoinMessge()
        {
            var join = new Join(new Node(
                Id.Of(1),
                new Name("node1"),
                Address.From("localhost:37371", AddressType.Op),
                Address.From("localhost:37372", AddressType.App)));
            MessageConverters.MessageToBytes(join, _messageBuffer);
            var raw = OperationalMessage.JOIN + "\n" + "id=1 nm=node1 op=localhost:37371 app=localhost:37372";
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(join, Join.From(raw));
        }

        [Fact]
        public void TestGenerateLeaderMessage()
        {
            var leader = new Leader(Id.Of(1));
            MessageConverters.MessageToBytes(leader, _messageBuffer);
            var raw = OperationalMessage.LEADER + "\nid=1";
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(leader, Leader.From(raw));
        }

        [Fact]
        public void TestGenerateLeaveMessage()
        {
            var leave = new Leave(Id.Of(1));
            MessageConverters.MessageToBytes(leave, _messageBuffer);
            var raw = OperationalMessage.LEAVE + "\nid=1";
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(leave, Leave.From(raw));
        }

        [Fact]
        public void TestGeneratePingMessage()
        {
            var ping = new Ping(Id.Of(1));
            MessageConverters.MessageToBytes(ping, _messageBuffer);
            var raw = OperationalMessage.PING + "\nid=1";
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(ping, Ping.From(raw));
        }

        [Fact]
        public void TestGeneratePulseMessage()
        {
            var pulse = new Pulse(Id.Of(1));
            MessageConverters.MessageToBytes(pulse, _messageBuffer);
            var raw = OperationalMessage.PULSE + "\nid=1";
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(pulse, Pulse.From(raw));
        }

        [Fact]
        public void TestGenerateSplitMessage()
        {
            var split = new Split(Id.Of(1));
            MessageConverters.MessageToBytes(split, _messageBuffer);
            var raw = OperationalMessage.SPLIT + "\nid=1";
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(split, Split.From(raw));
        }

        [Fact]
        public void TestGenerateVoteMessage()
        {
            var vote = new Vote(Id.Of(1));
            MessageConverters.MessageToBytes(vote, _messageBuffer);
            var raw = OperationalMessage.VOTE + "\nid=1";
            _expectedBuffer.Write(Converters.TextToBytes(raw));
            Assert.Equal(_expectedBuffer.ToArray(), _messageBuffer.ToArray());
    
            Assert.Equal(vote, Vote.From(raw));
        }

        public OperationalMessageGenerationTest()
        {
            _expectedBuffer.Clear();
            _messageBuffer.Clear();
            
            for (int idx = 0; idx < _bytes; ++idx)
            {
                _expectedBuffer.WriteByte( 0);
                _messageBuffer.WriteByte(0);
            }

            _expectedBuffer.Position = 0;
            _messageBuffer.Position = 0;
        }
    }
}