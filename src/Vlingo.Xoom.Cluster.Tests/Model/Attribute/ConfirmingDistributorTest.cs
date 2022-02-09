// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Linq;
using Vlingo.Xoom.Actors.TestKit;
using Vlingo.Xoom.Cluster.Model.Attribute;
using Vlingo.Xoom.Cluster.Model.Attribute.Message;
using Vlingo.Xoom.Cluster.Model.Message;
using Vlingo.Xoom.Cluster.Model.Outbound;
using Vlingo.Xoom.Cluster.Tests.Model.Outbound;
using Vlingo.Xoom.Common.Pool;
using Vlingo.Xoom.Wire.Fdx.Outbound;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Xoom.Cluster.Tests.Model.Attribute
{
    public class ConfirmingDistributorTest : AbstractClusterTest
    {
        private readonly MockManagedOutboundChannelProvider _channelProvider;
        private readonly ConfirmingDistributor _confirmingDistributor;
        private readonly Id _localNodeId;
        private readonly Node _localNode;
        private readonly AttributeSet _set;
        private readonly TrackedAttribute _tracked;

        [Fact]
        public void TestAcknowledgeConfirmation()
        {
            _confirmingDistributor.Distribute(_set, _tracked, ApplicationMessageType.AddAttribute);
    
            var trackingIds = _confirmingDistributor.AllTrackingIds;
    
            Assert.Single(trackingIds);
    
            var tackingId = trackingIds.ToList()[0];
    
            Assert.Equal(2, _confirmingDistributor.UnconfirmedNodesFor(tackingId).Count());
            _confirmingDistributor.AcknowledgeConfirmation(tackingId, _confirmingDistributor.UnconfirmedNodesFor(tackingId).ToList()[0]);
            Assert.Single(_confirmingDistributor.UnconfirmedNodesFor(tackingId));
            _confirmingDistributor.AcknowledgeConfirmation(tackingId, _confirmingDistributor.UnconfirmedNodesFor(tackingId).ToList()[0]);
            Assert.Empty(_confirmingDistributor.UnconfirmedNodesFor(tackingId));
        }

        [Fact]
        public void TestConfirmCreateAttributeSet()
        {
            _confirmingDistributor.ConfirmCreate("123", _set, _localNode);
    
            SingleChannelMessageAssertions();
    
            Assert.Equal(1, Application.InformAttributeSetCreatedCheck.Get());
        }
        
        [Fact]
        public void TestConfirmAddAttribute()
        {
            _confirmingDistributor.Confirm("123", _set, _tracked, ApplicationMessageType.AddAttribute, _localNode);
    
            SingleChannelMessageAssertions();
        }
        
        [Fact]
        public void TestConfirmReplaceAttribute()
        {
            _confirmingDistributor.Confirm("123", _set, _tracked, ApplicationMessageType.ReplaceAttribute, _localNode);
    
            SingleChannelMessageAssertions();
        }
        
        [Fact]
        public void TestConfirmRemoveAttribute()
        {
            _confirmingDistributor.Confirm("123", _set, _tracked, ApplicationMessageType.RemoveAttribute, _localNode);
    
            SingleChannelMessageAssertions();
        }
        
        [Fact]
        public void TestConfirmRemoveAttributeSet()
        {
            _confirmingDistributor.ConfirmRemove("123", _set, _localNode);
    
            SingleChannelMessageAssertions();
    
            Assert.Equal(1, Application.InformAttributeSetRemovedCheck.Get());
        }
        
        [Fact]
        public void TestDistributeCreateAttributeSet()
        {
            _confirmingDistributor.DistributeCreate(_set);
    
            MultiChannelMessageAssertions(2);
    
            Assert.Equal(1, Application.InformAttributeSetCreatedCheck.Get());
            Assert.Equal(1, Application.InformAttributeAddedCheck.Get());
        }

        [Fact]
        public void TestDistributeAddAttribute()
        {
            _confirmingDistributor.Distribute(_set, _tracked, ApplicationMessageType.AddAttribute);
    
            MultiChannelMessageAssertions(1);
    
            Assert.Equal(1, Application.InformAttributeAddedCheck.Get());
        }

        [Fact]
        public void TestDistributeReplaceAttribute()
        {
            _confirmingDistributor.Distribute(_set, _tracked, ApplicationMessageType.ReplaceAttribute);
    
            MultiChannelMessageAssertions(1);
    
            Assert.Equal(1, Application.InformAttributeReplacedCheck.Get());
        }
        
        [Fact]
        public void TestDistributeRemoveAttribute()
        {
            _confirmingDistributor.Distribute(_set, _tracked, ApplicationMessageType.RemoveAttribute);
    
            MultiChannelMessageAssertions(1);
    
            Assert.Equal(1, Application.InformAttributeRemovedCheck.Get());
        }
        
        [Fact]
        public void TestDistributeRemoveAttributeSet()
        {
            _confirmingDistributor.DistributeRemove(_set);
    
            MultiChannelMessageAssertions(2);
    
            Assert.Equal(1, Application.InformAttributeSetRemovedCheck.Get());
            Assert.Equal(1, Application.InformAttributeRemovedCheck.Get());
        }
        
        [Fact]
        public void TestRedistributeUnconfirmed()
        {
            var allOtherNodes = Config.AllOtherNodes(_localNodeId).ToList();

            var channel2 = _channelProvider.ChannelFor(allOtherNodes[0].Id);
            Mock(channel2).Until = TestUntil.Happenings(1);

            var channel3 = _channelProvider.ChannelFor(allOtherNodes[1].Id);
            Mock(channel3).Until = TestUntil.Happenings(1);
    
            var set = AttributeSet.Named("test-set");
            var tracked = set.AddIfAbsent(Attribute<string>.From("test-attr", "test-value"));
    
            _confirmingDistributor.Distribute(set, tracked, ApplicationMessageType.AddAttribute);
    
            _confirmingDistributor.RedistributeUnconfirmed();

            Mock(channel2).Until.Completes();
            Assert.Single(Mock(channel2).Writes);

            Mock(channel3).Until.Completes();
            Assert.Single(Mock(channel3).Writes);
        }
        
        public ConfirmingDistributorTest(ITestOutputHelper output) : base(output)
        {
            _localNodeId = Id.Of(1);
    
            _localNode = Config.NodeMatching(_localNodeId);
    
            _set = AttributeSet.Named("test-set");
    
            _tracked = _set.AddIfAbsent(Attribute<string>.From("test-attr", "test-value"));
    
            _channelProvider = new MockManagedOutboundChannelProvider(_localNodeId, Config);
    
            var pool = new ConsumerByteBufferPool(ElasticResourcePool<IConsumerByteBuffer, string>.Config.Of(10), Properties.OperationalBufferSize());

            var outboundStream = TestWorld.ActorFor<IOperationalOutboundStream>(
                () => new OperationalOutboundStreamActor(_localNode, _channelProvider, pool));

            _confirmingDistributor = new ConfirmingDistributor(Application, _localNode, outboundStream.Actor, Config);
        }

        private MockManagedOutboundChannel Mock(IManagedOutboundChannel channel) => (MockManagedOutboundChannel) channel;

        private void MultiChannelMessageAssertions(int messageCount)
        {
            var allOtherNodes = Config.AllOtherNodes(_localNodeId).ToList();
            var channel2 = _channelProvider.ChannelFor(allOtherNodes[0].Id);
            var channel3 = _channelProvider.ChannelFor(allOtherNodes[1].Id);
            Assert.Equal(messageCount, Mock(channel2).Writes.Count);
            Assert.Equal(messageCount, Mock(channel3).Writes.Count);
            var message2 = OperationalMessage.MessageFrom(Mock(channel2).Writes[0]);
            var message3 = OperationalMessage.MessageFrom(Mock(channel3).Writes[0]);
            Assert.True(message2.IsApp);
            Assert.True(message3.IsApp);
            Assert.Equal(_localNodeId, message2.Id);
            Assert.Equal(_localNodeId, message3.Id);
            Assert.Equal(message2, message3);
        }

        private void SingleChannelMessageAssertions()
        {
            var channel1 = _channelProvider.ChannelFor(_localNodeId);
            var message1 = OperationalMessage.MessageFrom(Mock(channel1).Writes[0]);

            Assert.Single(Mock(channel1).Writes);
            Assert.Equal(_localNodeId, message1.Id);
        }
    }
}