// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.IO;
using Vlingo.Cluster.Model.Message;
using Vlingo.Wire.Channel;
using Vlingo.Wire.Message;
using Xunit;

namespace Vlingo.Cluster.Tests.Model.Message
{
    using Vlingo.Wire.Node;
    
    public class RawMessageBuilderTest
    {
        private readonly MemoryStream _buffer;
        private readonly RawMessageBuilder _builder;
        private readonly Join _join;
        private readonly Leader _leader;
        private readonly Node _node1;
        private readonly Node _node2;

        [Fact]
        public void TestOneInboundMessage()
        {
            Join();
    
            _builder.PrepareContent().Sync();
            Assert.True(_builder.IsCurrentMessageComplete());
            var message = _builder.CurrentRawMessage();
            var text = message.AsTextMessage();
            var typed = OperationalMessage.MessageFrom(text);
            Assert.Equal(_join, typed);
        }

        [Fact]
        public void TestTwoInboundMessages()
        {
            Join();
            Leader();
    
            _builder.PrepareContent().Sync();
            Assert.True(_builder.IsCurrentMessageComplete());
            Assert.True(_builder.HasContent);
            var inboundJoin = _builder.CurrentRawMessage();
            var joinText = inboundJoin.AsTextMessage();
            var joinFromText = OperationalMessage.MessageFrom(joinText);
            Assert.Equal(_join, joinFromText);
    
            _builder.PrepareForNextMessage();
            Assert.True(_builder.HasContent);
            _builder.Sync();
    
            var inboundLeader = _builder.CurrentRawMessage();
            var leaderText = inboundLeader.AsTextMessage();
            var leaderFromText = OperationalMessage.MessageFrom(leaderText);
            Assert.Equal(_leader, leaderFromText);
        }

        public RawMessageBuilderTest()
        {
            _buffer = new MemoryStream(1000);
            _builder = new RawMessageBuilder(1000);
            _node1 = Node.With(Id.Of(1), Name.Of("node1"), Host.Of("localhost"), 37371, 37372);
            _node2 = Node.With(Id.Of(2), Name.Of("node2"), Host.Of("localhost"), 37373, 37374);
            _join = new Join(_node1);
            _leader = new Leader(_node2.Id);
        }
        
        private void Join()
        {
            _buffer.Clear();
            
            MessageConverters.MessageToBytes(_join, _buffer);
    
            _buffer.Flip();
    
            PrepareRawMessage(0, _buffer.Length);
        }

        private void Leader()
        {
            _buffer.Clear();
    
            MessageConverters.MessageToBytes(_leader, _buffer);
    
            _buffer.Flip();
    
            PrepareRawMessage(0, _buffer.Length);
        }
        
        private void PrepareRawMessage(int position, long bytesToAppend)
        {
            var inboundHeader = new RawMessageHeader(_node1.Id.Value, (short) 0, (short) _buffer.Length);
            var inboundMessage = new RawMessage(_buffer.Length);
            if (position == 0)
            {
                inboundMessage.Header(inboundHeader);
            }
            inboundMessage.Append(_buffer.ToArray(), position, bytesToAppend);
            inboundMessage.CopyBytesTo(_builder.WorkBuffer());
        }
    }
}