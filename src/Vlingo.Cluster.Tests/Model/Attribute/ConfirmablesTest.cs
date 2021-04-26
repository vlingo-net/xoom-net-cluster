// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Linq;
using Vlingo.Cluster.Model.Attribute;
using Vlingo.Cluster.Model.Attribute.Message;
using Vlingo.Xoom.Wire.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model.Attribute
{
    public class ConfirmablesTest : AbstractClusterTest
    {
        private readonly Confirmables _consumables;
        private readonly Node _localNode;
        private readonly Id _localNodeId;
        private readonly Node _remoteNode2;
        private readonly Node _remoteNode3;
        private readonly AttributeSet _set;
        private readonly TrackedAttribute _tracked;

        [Fact]
        public void TestCreateConfirmables()
        {
            Assert.Empty(_consumables.AllRedistributable);
    
            Assert.Empty(_consumables.AllTrackingIds);
    
            Assert.Equal(Confirmables.Confirmable.NoConfirmable, _consumables.ConfirmableOf("123"));
        }

        [Fact]
        public void TestConfirmConfirmables()
        {
            var addAttribute = AddAttribute.From(_localNode, _set, _tracked);
    
            _consumables.Unconfirmed(addAttribute);
    
            Assert.Single(_consumables.AllTrackingIds);
            Assert.Equal(2, _consumables.ConfirmableOf(addAttribute.TrackingId).UnconfirmedNodes.Count());
    
            _consumables.Confirm(addAttribute.TrackingId, _remoteNode2);
    
            Assert.Single(_consumables.AllTrackingIds);
            Assert.Single(_consumables.ConfirmableOf(addAttribute.TrackingId).UnconfirmedNodes);
    
            _consumables.Confirm(addAttribute.TrackingId, _remoteNode3);
    
            Assert.Empty(_consumables.AllTrackingIds);
        }

        [Fact]
        public void TestIsRedistributableAsOf()
        {
            var addAttribute = AddAttribute.From(_localNode, _set, _tracked);
    
            _consumables.Unconfirmed(addAttribute);
    
            Assert.False(_consumables.ConfirmableOf(addAttribute.TrackingId).IsRedistributableAsOf());
            Assert.Empty(_consumables.AllRedistributable);
    
            while (_consumables.AllRedistributable.Count() != 1);
            Assert.Single(_consumables.AllRedistributable);
        }

        [Fact]
        public void TestUnconfirmedConfirmables()
        {
            var addAttribute = AddAttribute.From(_localNode, _set, _tracked);
    
            _consumables.Unconfirmed(addAttribute);
    
            Assert.NotEmpty(_consumables.AllTrackingIds);
    
            Assert.NotNull(_consumables.ConfirmableOf(addAttribute.TrackingId));
        }
        
        public ConfirmablesTest(ITestOutputHelper output) : base(output)
        {
            _localNodeId = Id.Of(1);
    
            _localNode = Config.NodeMatching(_localNodeId);
    
            _remoteNode2 = Config.NodeMatching(Id.Of(2));
    
            _remoteNode3 = Config.NodeMatching(Id.Of(3));

            _set = AttributeSet.Named("test-set");

            _tracked = _set.AddIfAbsent(Attribute<string>.From("test-attr", "test-value"));

            _consumables = new Confirmables(_localNode, Config.AllOtherNodes(_localNodeId));
        }
    }
}