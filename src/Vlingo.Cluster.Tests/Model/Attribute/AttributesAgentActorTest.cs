// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
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
using Vlingo.Common;
using Vlingo.Common.Pool;
using Vlingo.Wire.Fdx.Inbound;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model.Attribute
{
    using Vlingo.Wire.Node;
    
    public class AttributesAgentActorTest : AbstractClusterTest
    {
        private readonly MockManagedOutboundChannelProvider _channelProvider;
        private readonly Id _localNodeId;
        private readonly Node _localNode;
        private readonly MockConfirmationInterest _interest;
        private readonly TestActor<IOperationalOutboundStream> _outboundStream;
        private readonly AttributeSet _set;
        private readonly TrackedAttribute _tracked;

        [Fact]
        public void TestAdd()
        {
            var agent =
                TestWorld.ActorFor<IAttributesAgent>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

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
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));
            
            agent.Actor.Add("test-set", "test-attr", "test-value1");
            agent.Actor.Replace("test-set", "test-attr", "test-value2");
    
            var allOtherNodes = Config.AllOtherNodes(_localNodeId).ToList();
            var channel2 = _channelProvider.ChannelFor(allOtherNodes[0].Id);
            var channel3 = _channelProvider.ChannelFor(allOtherNodes[1].Id);
            Assert.Equal(3, Mock(channel2).Writes.Count);
            Assert.Equal(3, Mock(channel3).Writes.Count);
    
            Assert.Equal("test-value2", Application.AttributesClient.Attribute<string>("test-set", "test-attr").Value);
        }
        
        [Fact]
        public void TestRemove()
        {
            var agent =
                TestWorld.ActorFor<IAttributesAgent>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));
            
            agent.Actor.Add("test-set", "test-attr", "test-value1");
            agent.Actor.Remove("test-set", "test-attr");
    
            var allOtherNodes = Config.AllOtherNodes(_localNodeId).ToList();
            var channel2 = _channelProvider.ChannelFor(allOtherNodes[0].Id);
            var channel3 = _channelProvider.ChannelFor(allOtherNodes[1].Id);
            Assert.Equal(3, Mock(channel2).Writes.Count);
            Assert.Equal(3, Mock(channel3).Writes.Count);
    
            Assert.Equal(Attribute<string>.Undefined, Application.AttributesClient.Attribute<string>("test-set", "test-attr"));
        }
        
        [Fact]
        public void TestRemoveAttributeSet()
        {
            var agent =
                TestWorld.ActorFor<IAttributesAgent>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));
            
            agent.Actor.Add("test-set", "test-attr1", "test-value1");
            agent.Actor.Add("test-set", "test-attr2", "test-value2");
            agent.Actor.Add("test-set", "test-attr3", "test-value3");
            agent.Actor.RemoveAll("test-set");
    
            var allOtherNodes = Config.AllOtherNodes(_localNodeId).ToList();
            var channel2 = _channelProvider.ChannelFor(allOtherNodes[0].Id);
            var channel3 = _channelProvider.ChannelFor(allOtherNodes[1].Id);
            
            
            // 1. create set, 2. add attr, 3. add attr, 4. add attr, 5. remove attr, 6. remove attr, 7. remove attr, 8. remove set
            Assert.Equal(8, Mock(channel2).Writes.Count);
            Assert.Equal(8, Mock(channel3).Writes.Count);
    
            Assert.Equal(Attribute<string>.Undefined, Application.AttributesClient.Attribute<string>("test-set", "test-attr1"));
            Assert.Equal(Attribute<string>.Undefined, Application.AttributesClient.Attribute<string>("test-set", "test-attr2"));
            Assert.Equal(Attribute<string>.Undefined, Application.AttributesClient.Attribute<string>("test-set", "test-attr3"));
            Assert.Empty(Application.AttributesClient.AllOf("test-set"));
            Assert.Empty(Application.AttributesClient.All);
        }

        [Fact]
        public void TestInboundStreamInterestCreateAttributeSet()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var message = CreateAttributeSet.From(_localNode, _set);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, message));
    
            var channel1 = _channelProvider.ChannelFor(_localNodeId);
            Assert.Single(Mock(channel1).Writes);
            Assert.Equal(1, Application.InformAttributeSetCreatedCheck.Get());
        }
        
        [Fact]
        public void TestInboundStreamInterestAddAttribute()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var message = AddAttribute.From(_localNode, _set, _tracked);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, message));
    
            var channel1 = _channelProvider.ChannelFor(_localNodeId);
            Assert.Single(Mock(channel1).Writes);
            Assert.Equal(1, Application.InformAttributeAddedCheck.Get());
        }
        
        [Fact]
        public void TestInboundStreamInterestReplaceAttribute()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var addMessage = AddAttribute.From(_localNode, _set, _tracked);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, addMessage));
            var replaceMessage = ReplaceAttribute.From(_localNode, _set, _tracked);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, replaceMessage));
    
            var channel1 = _channelProvider.ChannelFor(_localNodeId);
            Assert.Equal(2, Mock(channel1).Writes.Count);
            Assert.Equal(1, Application.InformAttributeAddedCheck.Get());
            Assert.Equal(1, Application.InformAttributeReplacedCheck.Get());
        }
        
        [Fact]
        public void TestInboundStreamInterestRemoveAttribute()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var addMessage = AddAttribute.From(_localNode, _set, _tracked);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, addMessage));
            var replaceMessage = RemoveAttribute.From(_localNode, _set, _tracked);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, replaceMessage));
    
            var channel1 = _channelProvider.ChannelFor(_localNodeId);
            Assert.Equal(2, Mock(channel1).Writes.Count);
            Assert.Equal(1, Application.InformAttributeAddedCheck.Get());
            Assert.Equal(1, Application.InformAttributeRemovedCheck.Get());
        }
        
        [Fact]
        public void TestInboundStreamInterestRemoveAttributeSet()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var createMessage = CreateAttributeSet.From(_localNode, _set);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, createMessage));
            var removeMessage = RemoveAttributeSet.From(_localNode, _set);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, removeMessage));
    
            var channel1 = _channelProvider.ChannelFor(_localNodeId);
            Assert.Equal(2, Mock(channel1).Writes.Count);
            Assert.Equal(1, Application.InformAttributeSetCreatedCheck.Get());
            Assert.Equal(1, Application.InformAttributeSetRemovedCheck.Get());
        }
        
        [Fact]
        public void TestConfirmCreateAttributeSet()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var confirm = new ConfirmCreateAttributeSet("123", _localNode, _set);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, confirm));
            Assert.Equal(1, _interest.Confirmed);
            Assert.Equal(_set.Name, _interest.AttributeSetName);
            Assert.Equal(confirm.Type, _interest.Type);
        }
        
        [Fact]
        public void TestConfirmAddAttribute()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var confirm = new ConfirmAttribute("123", _localNode, _set, _tracked, ApplicationMessageType.ConfirmAddAttribute);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, confirm));
            Assert.Equal(_set.Name, _interest.AttributeSetName);
            Assert.Equal(_tracked.Attribute.Name, _interest.AttributeName);
            Assert.Equal(confirm.Type, _interest.Type);
            Assert.Equal(1, _interest.Confirmed);
        }
        
        [Fact]
        public void TestConfirmReplaceAttribute()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var confirm = new ConfirmAttribute("123", _localNode, _set, _tracked, ApplicationMessageType.ConfirmReplaceAttribute);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, confirm));
            Assert.Equal(_set.Name, _interest.AttributeSetName);
            Assert.Equal(_tracked.Attribute.Name, _interest.AttributeName);
            Assert.Equal(confirm.Type, _interest.Type);
            Assert.Equal(1, _interest.Confirmed);
        }
        
        [Fact]
        public void TestConfirmRemoveAttribute()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var confirm = new ConfirmAttribute("123", _localNode, _set, _tracked, ApplicationMessageType.ConfirmRemoveAttribute);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, confirm));
            Assert.Equal(_set.Name, _interest.AttributeSetName);
            Assert.Equal(_tracked.Attribute.Name, _interest.AttributeName);
            Assert.Equal(confirm.Type, _interest.Type);
            Assert.Equal(1, _interest.Confirmed);
        }
        
        [Fact]
        public void TestConfirmRemoveAttributeSet()
        {
            var inboundStreamInterest =
                TestWorld.ActorFor<IInboundStreamInterest>(
                    () => new AttributesAgentActor(_localNode, Application, _outboundStream.Actor, Config, _interest));

            var confirm = new ConfirmRemoveAttributeSet("123", _localNode, _set);
            inboundStreamInterest.Actor.HandleInboundStreamMessage(AddressType.Op, RawMessageFor(_localNodeId, _localNode.Name, confirm));
            Assert.Equal(1, _interest.Confirmed);
            Assert.Equal(_set.Name, _interest.AttributeSetName);
            Assert.Equal(confirm.Type, _interest.Type);
        }
        
        public AttributesAgentActorTest(ITestOutputHelper output) : base(output)
        {
            _localNodeId = Id.Of(1);
    
            _localNode = Config.NodeMatching(_localNodeId);
    
            _set = AttributeSet.Named("test-set");
    
            _tracked = _set.AddIfAbsent(Attribute<string>.From("test-attr", "test-value"));
    
            _channelProvider = new MockManagedOutboundChannelProvider(_localNodeId, Config);
    
            var pool = new ConsumerByteBufferPool(ElasticResourcePool<IConsumerByteBuffer, string>.Config.Of(10), Properties.OperationalBufferSize());
    
            _interest = new MockConfirmationInterest();

            _outboundStream =
                TestWorld.ActorFor<IOperationalOutboundStream>(
                    () => new OperationalOutboundStreamActor(_localNode, _channelProvider, pool));
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