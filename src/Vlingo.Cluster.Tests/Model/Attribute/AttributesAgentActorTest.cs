// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.IO;
using System.Linq;
using Vlingo.Actors;
using Vlingo.Actors.TestKit;
using Vlingo.Cluster.Model.Attribute;
using Vlingo.Cluster.Model.Attribute.Message;
using Vlingo.Cluster.Model.Message;
using Vlingo.Cluster.Model.Outbound;
using Vlingo.Cluster.Tests.Model.Outbound;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model.Attribute
{
    using Vlingo.Wire.Node;
    
    public class AttributesAgentActorTest : AbstractClusterTest
    {
        private MockManagedOutboundChannelProvider _channelProvider;
        private Id _localNodeId;
        private Node _localNode;
        private MockConfirmationInterest _interest;
        private ByteBufferPool _pool;
        private TestActor<IOperationalOutboundStream> _outboundStream;
        private AttributeSet _set;
        private TrackedAttribute _tracked;

        [Fact]
        public void TestAdd()
        {
            var agent =
                TestWorld.ActorFor<IAttributesAgent>(
                    Definition.Has<AttributesAgentActor>(
                        Definition.Parameters(_localNode, Application, _outboundStream.Actor, Config, _interest)));
            
            agent.Actor.Add("test-set", "test-attr", "test-value");
    
            var allOtherNodes = Config.AllOtherNodes(_localNodeId).ToList();
            var channel2 = _channelProvider.ChannelFor(allOtherNodes[0].Id);
            var channel3 = _channelProvider.ChannelFor(allOtherNodes[1].Id);
            Assert.Equal(2, Mock(channel2).Writes.Count);
            Assert.Equal(2, Mock(channel3).Writes.Count);
    
            Assert.Equal("test-value", Application.AttributesClient.Attribute<string>("test-set", "test-attr").Value);
        }
        
        [Fact]
        public void TestReplace()
        {
            var agent =
                TestWorld.ActorFor<IAttributesAgent>(
                    Definition.Has<AttributesAgentActor>(
                        Definition.Parameters(_localNode, Application, _outboundStream.Actor, Config, _interest)));
            
            agent.Actor.Add("test-set", "test-attr", "test-value1");
            agent.Actor.Replace("test-set", "test-attr", "test-value2");
    
            var allOtherNodes = Config.AllOtherNodes(_localNodeId).ToList();
            var channel2 = _channelProvider.ChannelFor(allOtherNodes[0].Id);
            var channel3 = _channelProvider.ChannelFor(allOtherNodes[1].Id);
            Assert.Equal(3, Mock(channel2).Writes.Count);
            Assert.Equal(3, Mock(channel3).Writes.Count);
    
            Assert.Equal("test-value2", Application.AttributesClient.Attribute<string>("test-set", "test-attr").Value);
        }
        
        public AttributesAgentActorTest(ITestOutputHelper output) : base(output)
        {
            _localNodeId = Id.Of(1);
    
            _localNode = Config.NodeMatching(_localNodeId);
    
            _set = AttributeSet.Named("test-set");
    
            _tracked = _set.AddIfAbsent(Attribute<string>.From("test-attr", "test-value"));
    
            _channelProvider = new MockManagedOutboundChannelProvider(_localNodeId, Config);
    
            _pool = new ByteBufferPool(10, Properties.OperationalBufferSize());
    
            _interest = new MockConfirmationInterest();
    
            _outboundStream =
                TestWorld.ActorFor<IOperationalOutboundStream>(
                    Definition.Has<OperationalOutboundStreamActor>(
                        Definition.Parameters(_localNode, _channelProvider, _pool)));
        }
        
        private RawMessage RawMessageFor(Id id, Name name, ApplicationMessage message) {
            var messageBuffer = new MemoryStream(4096);
            var says = ApplicationSays.From(id, name, message.ToPayload());
            MessageConverters.MessageToBytes(says, messageBuffer);
            return Converters.ToRawMessage(Id.Of(1).Value, messageBuffer);
        }

        private MockManagedOutboundChannel Mock(IManagedOutboundChannel channel) => (MockManagedOutboundChannel) channel;
    }
}