// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Cluster.Model.Attribute;
using Vlingo.Xoom.Wire.Fdx.Outbound;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Application
{
    public class FakeClusterApplicationActor : ClusterApplicationAdapter
    {
        private IAttributesProtocol? _client;
        private readonly Node _localNode;
        private IApplicationOutboundStream? _responder;

        public FakeClusterApplicationActor(Node localNode)
        {
            _localNode = localNode;
        }

        public override void Start() =>
            Logger.Debug($"APP: ClusterApplication started on node: {_localNode}");

        public override void HandleApplicationMessage(RawMessage message) =>
            Logger.Debug($"APP: Received application message: {message.AsTextMessage()}");

        public override void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster)
        {
            foreach (var id in liveNodes)
            {
                Logger.Debug($"APP: Live node confirmed: {id}");
            }
            
            PrintHealthy(isHealthyCluster);
        }

        public override void InformLeaderElected(Id leaderId, bool isHealthyCluster, bool isLocalNodeLeading)
        {
            Logger.Debug($"APP: Leader elected: {leaderId}");
            PrintHealthy(isHealthyCluster);
            if (isLocalNodeLeading)
            {
                Logger.Debug("APP: Local node is leading.");
            }
        }

        public override void InformLeaderLost(Id lostLeaderId, bool isHealthyCluster)
        {
            Logger.Debug($"APP: Leader lost: {lostLeaderId}");
            PrintHealthy(isHealthyCluster);
        }

        public override void InformLocalNodeShutDown(Id nodeId) => Logger.Debug($"APP: Local node shut down: {nodeId}");

        public override void InformLocalNodeStarted(Id nodeId) => Logger.Debug($"APP: Local node started: {nodeId}");

        public override void InformNodeIsHealthy(Id nodeId, bool isHealthyCluster)
        {
            Logger.Debug($"APP: Node reported healthy: {nodeId}");
            PrintHealthy(isHealthyCluster);
        }

        public override void InformNodeJoinedCluster(Id nodeId, bool isHealthyCluster)
        {
            Logger.Debug($"APP: {nodeId} joined cluster");
            PrintHealthy(isHealthyCluster);
        }

        public override void InformNodeLeftCluster(Id nodeId, bool isHealthyCluster)
        {
            Logger.Debug($"APP: {nodeId} left cluster");
            PrintHealthy(isHealthyCluster);
        }

        public override void InformQuorumAchieved()
        {
            Logger.Debug("APP: Quorum achieved");
            PrintHealthy(true);
        }

        public override void InformQuorumLost()
        {
            Logger.Debug("APP: Quorum lost");
            PrintHealthy(false);
        }

        public override void InformResponder(IApplicationOutboundStream? responder)
        {
            _responder = responder;
            Logger.Debug($"APP: Informed of responder: {_responder}");
        }

        public override void InformAttributesClient(IAttributesProtocol client)
        {
            Logger.Debug("APP: Attributes Client received.");
            _client = client;
            if (_localNode.Id.Value == 1)
            {
                _client.Add("fake.set", "fake.attribute.name1", "value1");
                _client.Add("fake.set", "fake.attribute.name2", "value2");
            }
        }

        public override void InformAttributeSetCreated(string? attributeSetName) =>
            Logger.Debug($"APP: Attributes Set Created: {attributeSetName}");

        public override void InformAttributeAdded(string attributeSetName, string? attributeName)
        {
            var attr = _client?.Attribute<string>(attributeSetName, attributeName);
            Logger.Debug($"APP: Attribute Set {attributeSetName} Attribute Added: {attributeName} Value: {attr?.Value}");
            if (_localNode.Id.Value == 1)
            {
                _client?.Replace("fake.set", "fake.attribute.name1", "value-replaced-2");
                _client?.Replace("fake.set", "fake.attribute.name2", "value-replaced-20");
            }
        }

        public override void InformAttributeRemoved(string attributeSetName, string? attributeName)
        {
            var attr = _client?.Attribute<string>(attributeSetName, attributeName);
            Logger.Debug($"APP: Attribute Set {attributeSetName} Attribute Removed: {attributeName} Attribute: {attr}");
        }

        public override void InformAttributeSetRemoved(string? attributeSetName) =>
            Logger.Debug($"APP: Attributes Set Removed: {attributeSetName}");

        public override void InformAttributeReplaced(string attributeSetName, string? attributeName)
        {
            var attr = _client?.Attribute<string>(attributeSetName, attributeName);
            Logger.Debug($"APP: Attribute Set {attributeSetName} Attribute Replaced: {attributeName} Value: {attr?.Value}");
            if (_localNode.Id.Value == 1)
            {
                _client?.Remove("fake.set", "fake.attribute.name1");
            }
        }
        
        private void PrintHealthy(bool isHealthyCluster)
        {
            if (isHealthyCluster)
            {
                Logger.Debug("APP: Cluster is healthy");
            }
            else
            {
                Logger.Debug("APP: Cluster is NOT healthy");
            }
        }
    }
}