// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model.Outbound
{
    using Vlingo.Wire.Fdx.Outbound;
    
    public class OutboundTest : AbstractClusterTest
    {
        private static string _message1 = "Message1";
        private static string _message2 = "Message2";
        private static string _message3 = "Message3";
  
        private readonly MockManagedOutboundChannelProvider _channelProvider;
        private readonly ByteBufferPool _pool;
        private readonly Outbound _outbound;

        [Fact]
        public async Task TestBroadcast()
        {
            var buffer = new MemoryStream(Properties.OperationalBufferSize());
    
            var rawMessage1 = BuildRawMessageBuffer(buffer, _message1);
            var rawMessage2 = BuildRawMessageBuffer(buffer, _message2);
            var rawMessage3 = BuildRawMessageBuffer(buffer, _message3);
    
            await _outbound.Broadcast(rawMessage1);
            await _outbound.Broadcast(rawMessage2);
            await _outbound.Broadcast(rawMessage3);

            foreach (var channel in _channelProvider.AllOtherNodeChannels.Values)
            {
                var mock = (MockManagedOutboundChannel) channel;
      
                Assert.Equal(_message1, mock.Writes[0]);
                Assert.Equal(_message2, mock.Writes[1]);
                Assert.Equal(_message3, mock.Writes[2]);
            }
        }

        [Fact]
        public async Task TestBroadcastPooledByteBuffer()
        {
            var buffer1 = _pool.Access();
            var buffer2 = _pool.Access();
            var buffer3 = _pool.Access();
    
            var rawMessage1 = BuildRawMessageBuffer((MemoryStream)buffer1.AsStream(), _message1);
            BytesFrom(rawMessage1, (MemoryStream)buffer1.AsStream());
            var rawMessage2 = BuildRawMessageBuffer((MemoryStream)buffer2.AsStream(), _message2);
            BytesFrom(rawMessage2, (MemoryStream)buffer2.AsStream());
            var rawMessage3 = BuildRawMessageBuffer((MemoryStream)buffer3.AsStream(), _message3);
            BytesFrom(rawMessage3, (MemoryStream)buffer3.AsStream());
            
            await _outbound.Broadcast(buffer1);
            await _outbound.Broadcast(buffer2);
            await _outbound.Broadcast(buffer3);
            
            foreach (var channel in _channelProvider.AllOtherNodeChannels.Values)
            {
                var mock = (MockManagedOutboundChannel) channel;
      
                Assert.Equal(_message1, mock.Writes[0]);
                Assert.Equal(_message2, mock.Writes[1]);
                Assert.Equal(_message3, mock.Writes[2]);
            }
        }
        
        [Fact]
        public async Task TestBroadcastToSelectNodes()
        {
            var buffer = new MemoryStream(Properties.OperationalBufferSize());
    
            var rawMessage1 = BuildRawMessageBuffer(buffer, _message1);
            var rawMessage2 = BuildRawMessageBuffer(buffer, _message2);
            var rawMessage3 = BuildRawMessageBuffer(buffer, _message3);

            var selectNodes = new List<Wire.Node.Node>();
            selectNodes.Add(Config.NodeMatching(Id.Of(3)));
            
            await _outbound.Broadcast(selectNodes, rawMessage1);
            await _outbound.Broadcast(selectNodes, rawMessage2);
            await _outbound.Broadcast(selectNodes, rawMessage3);
            
            var mock = (MockManagedOutboundChannel) _channelProvider.ChannelFor(Id.Of(3));
            
            Assert.Equal(_message1, mock.Writes[0]);
            Assert.Equal(_message2, mock.Writes[1]);
            Assert.Equal(_message3, mock.Writes[2]);
        }
        
        [Fact]
        public async Task TestSendTo()
        {
            var buffer = new MemoryStream(Properties.OperationalBufferSize());
    
            var rawMessage1 = BuildRawMessageBuffer(buffer, _message1);
            var rawMessage2 = BuildRawMessageBuffer(buffer, _message2);
            var rawMessage3 = BuildRawMessageBuffer(buffer, _message3);

            var id3 = Id.Of(3);
            
            await _outbound.SendTo(rawMessage1, id3);
            await _outbound.SendTo(rawMessage2, id3);
            await _outbound.SendTo(rawMessage3, id3);
            
            var mock = (MockManagedOutboundChannel) _channelProvider.ChannelFor(Id.Of(3));
            
            Assert.Equal(_message1, mock.Writes[0]);
            Assert.Equal(_message2, mock.Writes[1]);
            Assert.Equal(_message3, mock.Writes[2]);
        }
        
        [Fact]
        public async Task TestSendToPooledByteBuffer()
        {
            var buffer1 = _pool.Access();
            var buffer2 = _pool.Access();
            var buffer3 = _pool.Access();
    
            var rawMessage1 = BuildRawMessageBuffer((MemoryStream)buffer1.AsStream(), _message1);
            BytesFrom(rawMessage1, (MemoryStream)buffer1.AsStream());
            var rawMessage2 = BuildRawMessageBuffer((MemoryStream)buffer2.AsStream(), _message2);
            BytesFrom(rawMessage2, (MemoryStream)buffer2.AsStream());
            var rawMessage3 = BuildRawMessageBuffer((MemoryStream)buffer3.AsStream(), _message3);
            BytesFrom(rawMessage3, (MemoryStream)buffer3.AsStream());
            
            var id3 = Id.Of(3);
            
            await _outbound.SendTo(buffer1, id3);
            await _outbound.SendTo(buffer2, id3);
            await _outbound.SendTo(buffer3, id3);
            
            var mock = (MockManagedOutboundChannel) _channelProvider.ChannelFor(Id.Of(3));
            
            Assert.Equal(_message1, mock.Writes[0]);
            Assert.Equal(_message2, mock.Writes[1]);
            Assert.Equal(_message3, mock.Writes[2]);
        }
        
        public OutboundTest(ITestOutputHelper output) : base(output)
        {
            _pool = new ByteBufferPool(10, Properties.OperationalBufferSize());
            _channelProvider = new MockManagedOutboundChannelProvider(Id.Of(1), Config);
            _outbound = new Outbound(_channelProvider, new ByteBufferPool(10, 10_000));
        }
    }
}