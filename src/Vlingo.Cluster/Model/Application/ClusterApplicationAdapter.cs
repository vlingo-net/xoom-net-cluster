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

        public abstract void HandleApplicationMessage(RawMessage message, IApplicationOutboundStream responder);

        public abstract void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster);

        public abstract void InformLeaderElected(Id leaderId, bool isHealthyCluster, bool isLocalNodeLeading);

        public abstract void InformLeaderLost(Id lostLeaderId, bool isHealthyCluster);

        public abstract void InformLocalNodeShutDown(Id nodeId);

        public abstract void InformLocalNodeStarted(Id nodeId);

        public abstract void InformNodeIsHealthy(Id nodeId, bool isHealthyCluster);

        public abstract void InformNodeJoinedCluster(Id nodeId, bool isHealthyCluster);

        public abstract void InformNodeLeftCluster(Id nodeId, bool isHealthyCluster);

        public abstract void InformQuorumAchieved();

        public abstract void InformQuorumLost();

        public abstract void InformAttributesClient(IAttributesProtocol client);

        public abstract void InformAttributeSetCreated(string attributeSetName);

        public abstract void InformAttributeAdded(string attributeSetName, string attributeName);

        public abstract void InformAttributeRemoved(string attributeSetName, string attributeName);

        public abstract void InformAttributeSetRemoved(string attributeSetName);

        public abstract void InformAttributeReplaced(string attributeSetName, string attributeName);
    }
}