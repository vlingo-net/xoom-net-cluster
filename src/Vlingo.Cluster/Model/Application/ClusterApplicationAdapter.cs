// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Cluster.Model.Attribute;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Application
{
    public abstract class ClusterApplicationAdapter : ClusterApplicationActor, IClusterApplication
    {
        public override bool IsStopped => false;

        public override void Start()
        {
        }

        public override void Stop()
        {
        }

        public void HandleApplicationMessage(RawMessage message, IApplicationOutboundStream responder)
        {
        }

        public void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster)
        {
        }

        public void InformLeaderElected(Id leaderId, bool isHealthyCluster, bool isLocalNodeLeading)
        {
        }

        public void InformLeaderLost(Id lostLeaderId, bool isHealthyCluster)
        {
        }

        public void InformLocalNodeShutDown(Id nodeId)
        {
        }

        public void InformLocalNodeStarted(Id nodeId)
        {
        }

        public void InformNodeIsHealthy(Id nodeId, bool isHealthyCluster)
        {
        }

        public void InformNodeJoinedCluster(Id nodeId, bool isHealthyCluster)
        {
        }

        public void InformNodeLeftCluster(Id nodeId, bool isHealthyCluster)
        {
        }

        public void InformQuorumAchieved()
        {
        }

        public void InformQuorumLost()
        {
        }

        public void InformAttributesClient(IAttributesProtocol client)
        {
        }

        public void InformAttributeSetCreated(string attributeSetName)
        {
        }

        public void InformAttributeAdded(string attributeSetName, string attributeName)
        {
        }

        public void InformAttributeRemoved(string attributeSetName, string attributeName)
        {
        }

        public void InformAttributeSetRemoved(string attributeSetName)
        {
        }

        public void InformAttributeReplaced(string attributeSetName, string attributeName)
        {
        }
    }
}