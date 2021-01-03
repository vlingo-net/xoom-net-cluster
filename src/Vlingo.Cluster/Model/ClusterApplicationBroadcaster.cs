// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Application;
using Vlingo.Cluster.Model.Attribute;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model
{
    internal class ClusterApplicationBroadcaster : IClusterApplication
    {
        private readonly List<IClusterApplication> _clusterApplications;
        private readonly ILogger _logger;

        internal ClusterApplicationBroadcaster(ILogger logger)
        {
            _logger = logger;
            _clusterApplications = new List<IClusterApplication>();
        }

        public void RegisterClusterApplication(IClusterApplication clusterApplication) =>
            _clusterApplications.Add(clusterApplication);
            
        //========================================
        // ClusterApplication
        //========================================
        
        public bool IsStopped => false;
        
        public void Start()
        {
        }
        
        public void Conclude()
        {
        }

        public void Stop()
        {
        }
        
        public void HandleApplicationMessage(RawMessage message)
            => Broadcast(app => app.HandleApplicationMessage(message));

        public void InformAllLiveNodes(IEnumerable<Wire.Node.Node> liveNodes, bool isHealthyCluster) =>
            Broadcast(app => app.InformAllLiveNodes(liveNodes, isHealthyCluster));

        public void InformLeaderElected(Id leaderId, bool isHealthyCluster, bool isLocalNodeLeading) =>
            Broadcast(app => app.InformLeaderElected(leaderId, isHealthyCluster, isLocalNodeLeading));

        public void InformLeaderLost(Id lostLeaderId, bool isHealthyCluster) =>
            Broadcast(app => app.InformLeaderLost(lostLeaderId, isHealthyCluster));

        public void InformLocalNodeShutDown(Id nodeId) => Broadcast(app => app.InformLocalNodeShutDown(nodeId));

        public void InformLocalNodeStarted(Id nodeId) => Broadcast(app => app.InformLocalNodeStarted(nodeId));

        public void InformNodeIsHealthy(Id nodeId, bool isHealthyCluster) =>
            Broadcast(app => app.InformNodeIsHealthy(nodeId, isHealthyCluster));

        public void InformNodeJoinedCluster(Id nodeId, bool isHealthyCluster) =>
            Broadcast(app => app.InformNodeJoinedCluster(nodeId, isHealthyCluster));

        public void InformNodeLeftCluster(Id nodeId, bool isHealthyCluster) =>
            Broadcast(app => app.InformNodeLeftCluster(nodeId, isHealthyCluster));

        public void InformQuorumAchieved() => Broadcast(app => app.InformQuorumAchieved());

        public void InformQuorumLost() => Broadcast(app => app.InformQuorumLost());
        public void InformResponder(IApplicationOutboundStream? responder)
            => Broadcast(app => app.InformResponder(responder));

        public void InformAttributesClient(IAttributesProtocol client) =>
            Broadcast(app => app.InformAttributesClient(client));

        public void InformAttributeSetCreated(string? attributeSetName) =>
            Broadcast(app => app.InformAttributeSetCreated(attributeSetName));

        public void InformAttributeAdded(string attributeSetName, string? attributeName) =>
            Broadcast(app => app.InformAttributeAdded(attributeSetName, attributeName));

        public void InformAttributeRemoved(string attributeSetName, string? attributeName) =>
            Broadcast(app => app.InformAttributeRemoved(attributeSetName, attributeName));

        public void InformAttributeSetRemoved(string? attributeSetName) =>
            Broadcast(app => app.InformAttributeSetRemoved(attributeSetName));

        public void InformAttributeReplaced(string attributeSetName, string? attributeName) =>
            Broadcast(app => app.InformAttributeReplaced(attributeSetName, attributeName));

        private void Broadcast(Action<IClusterApplication> inform)
        {
            foreach (var app in _clusterApplications)
            {
                try
                {
                    inform(app);
                }
                catch (Exception e)
                {
                    _logger.Error($"Cannot inform because: {e.Message}", e);
                }
            }
        }
    }
}