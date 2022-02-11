// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model.Attribute;
using Vlingo.Xoom.Wire.Fdx.Outbound;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Application;

public interface IClusterApplication : IStartable, IStoppable
{
    void HandleApplicationMessage(RawMessage message);

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
        
    void InformResponder(IApplicationOutboundStream? responder);

    void InformAttributesClient(IAttributesProtocol client);
        
    void InformAttributeSetCreated(string? attributeSetName);
        
    void InformAttributeAdded(string attributeSetName, string? attributeName);
            
    void InformAttributeRemoved(string attributeSetName, string? attributeName);
            
    void InformAttributeSetRemoved(string? attributeSetName);
        
    void InformAttributeReplaced(string attributeSetName, string? attributeName);
}

public static class ClusterApplicationFactory
{
    public static IClusterApplication Instance<TActor>(Stage applicationStage,
        Expression<Func<TActor>> instantiator) => 
        applicationStage.ActorFor<IClusterApplication>(Definition.Has(instantiator, "cluster-application"));
}