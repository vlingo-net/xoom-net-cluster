// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Attribute;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Application
{
    public interface IClusterApplication : IStartable, IStoppable
    {
        void HandleApplicationMessage(RawMessage message, IApplicationOutboundStream responder);

        void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster);

        void InformLeaderElected(Id leaderId, bool isHealthyCluster, bool isLocalNodeLeading);
        
        void InformLeaderLost(Id lostLeaderId, bool isHealthyCluster);
            
        void InformLocalNodeShutDown(Id nodeId);
        
        void InformLocalNodeStarted(Id nodeId);
        
        void InformNodeIsHealthy(Id nodeId, bool isHealthyCluster);
            
        void InformNodeJoinedCluster(Id nodeId, bool isHealthyCluster);
            
        void InformNodeLeftCluster(Id nodeId, bool isHealthyCluster);
            
        void InformQuorumAchieved();
        
        void InformQuorumLost();

        void InformAttributesClient(IAttributesProtocol client);
        
        void InformAttributeSetCreated(string attributeSetName);
        
        void InformAttributeAdded(string attributeSetName, string attributeName);
            
        void InformAttributeRemoved(string attributeSetName, string attributeName);
            
        void InformAttributeSetRemoved(string attributeSetName);
        
        void InformAttributeReplaced(string attributeSetName, string attributeName);
    }

    public static class ClusterApplicationFactory
    {
        public static IClusterApplication Instance(World world, Node node)
        {
            var clusterApplicationActor = Properties.Instance.ClusterApplicationType();
            var applicationStage = world.StageNamed(Properties.Instance.ClusterApplicationStageName());
            
            return applicationStage
                .ActorFor<IClusterApplication>(
                    Definition.Has(clusterApplicationActor,
                        Definition.Parameters(node), "cluster-application"));
        }
    }
}