// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.IO;
using Vlingo.Actors;
using Vlingo.Actors.TestKit;
using Vlingo.Common;
using Vlingo.Common.Pool;
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
        
        private readonly MockManagedOutboundChannelProvider _channelProvider;
        private readonly TestActor<IApplicationOutboundStream> _outboundStream;
        private readonly TestWorld _world;

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
    
            var localNodeId = Id.Of(1);
    
            _channelProvider = new MockManagedOutboundChannelProvider(localNodeId, Config);
    
            var pool = new ConsumerByteBufferPool(ElasticResourcePool<IConsumerByteBuffer, string>.Config.Of(10), Properties.OperationalBufferSize());

            _outboundStream =
                _world.ActorFor<IApplicationOutboundStream>(
                    () => new ApplicationOutboundStreamActor(_channelProvider, pool));
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