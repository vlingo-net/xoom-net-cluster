// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.IO;
using Vlingo.Actors;
using Vlingo.Actors.TestKit;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model.Outbound
{
    public class ApplicationOutboundStreamTest : AbstractClusterTest
    {
        private static string _message1 = "Message1";
        
        private MockManagedOutboundChannelProvider _channelProvider;
        private Id _localNodeId;
        private ByteBufferPool _pool;
        private TestActor<IApplicationOutboundStream> _outboundStream;
        private TestWorld _world;

        [Fact]
        public void TestBroadcast()
        {
            var buffer = new MemoryStream(Properties.OperationalBufferSize());
    
            var rawMessage1 = BuildRawMessageBuffer(buffer, _message1);
    
            _outboundStream.Actor.Broadcast(rawMessage1);

            foreach (var channel in AllTargetChannels())
            {
                Assert.Equal(_message1, Mock(channel).Writes[0]);
            }
        }

        [Fact]
        public void TestSendTo()
        {
            var targetId = Id.Of(3);
    
            var buffer = new MemoryStream(Properties.OperationalBufferSize());
    
            var rawMessage1 = BuildRawMessageBuffer(buffer, _message1);
    
            _outboundStream.Actor.SendTo(rawMessage1, targetId);
    
            Assert.Equal(_message1, Mock(_channelProvider.ChannelFor(targetId)).Writes[0]);
    
            var anotherTargetId = Id.Of(2);
    
            _outboundStream.Actor.SendTo(rawMessage1, anotherTargetId);
    
            Assert.Equal(_message1, Mock(_channelProvider.ChannelFor(anotherTargetId)).Writes[0]);
        }


        public ApplicationOutboundStreamTest(ITestOutputHelper output) : base(output)
        {
            _world = TestWorld.Start("test-outbound-stream");
    
            _localNodeId = Id.Of(1);
    
            _channelProvider = new MockManagedOutboundChannelProvider(_localNodeId, Config);
    
            _pool = new ByteBufferPool(10, Properties.OperationalBufferSize());
    
            _outboundStream =
                _world.ActorFor<IApplicationOutboundStream>(
                    Definition.Has<ApplicationOutboundStreamActor>(
                        Definition.Parameters(_channelProvider, _pool)));
        }

        public override void Dispose()
        {
            _world.Terminate();
            base.Dispose();
        }
        
        private MockManagedOutboundChannel Mock(IManagedOutboundChannel channel)
        {
            return (MockManagedOutboundChannel) channel;
        }

        private List<IManagedOutboundChannel> AllTargetChannels()
        {
            return new List<IManagedOutboundChannel>(_channelProvider.AllOtherNodeChannels.Values);
        }
    }
}