// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Cluster.Model.Application;
using Vlingo.Cluster.Model.Attribute;
using Vlingo.Common;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Tests.Model
{   
    public class MockClusterApplication : IClusterApplication
    {
        public MockClusterApplication()
        {
            AllLiveNodes = new AtomicInteger(0);
        }
        
        public IAttributesProtocol AttributesClient { get; private set; }
  
        public AtomicInteger AllLiveNodes { get; }
        
        public AtomicInteger HandleApplicationMessageCheck => new AtomicInteger(0);
  
        public AtomicInteger InformLeaderElectedCheck => new AtomicInteger(0);
        
        public AtomicInteger InformLeaderLostCheck => new AtomicInteger(0);
        
        public AtomicInteger InformLocalNodeShutDownCheck => new AtomicInteger(0);
        
        public AtomicInteger InformLocalNodeStartedCheck => new AtomicInteger(0);
        
        public AtomicInteger InformNodeIsHealthyCheck => new AtomicInteger(0);
        
        public AtomicInteger InformNodeJoinedClusterCheck => new AtomicInteger(0);
        
        public AtomicInteger InformNodeLeftClusterCheck => new AtomicInteger(0);
        
        public AtomicInteger InformQuorumAchievedCheck => new AtomicInteger(0);
        
        public AtomicInteger InformQuorumLostCheck => new AtomicInteger(0);
  
        public AtomicInteger InformAttributesClientCheck => new AtomicInteger(0);
        
        public AtomicInteger InformAttributeSetCreatedCheck => new AtomicInteger(0);
        
        public AtomicInteger InformAttributeAddedCheck => new AtomicInteger(0);
        
        public AtomicInteger InformAttributeRemovedCheck => new AtomicInteger(0);
        
        public AtomicInteger InformAttributeReplacedCheck => new AtomicInteger(0);
        
        public AtomicInteger InformAttributeSetRemovedCheck => new AtomicInteger(0);
  
        public AtomicInteger StopCheck => new AtomicInteger(0);
        
        public void Start()
        {
        }

        public void Stop() => StopCheck.IncrementAndGet();

        public bool IsStopped => false;

        public void HandleApplicationMessage(RawMessage message, IApplicationOutboundStream responder) =>
            HandleApplicationMessageCheck.IncrementAndGet();

        public void InformAllLiveNodes(IEnumerable<Wire.Node.Node> liveNodes, bool isHealthyCluster) =>
            AllLiveNodes.IncrementAndGet();

        public void InformLeaderElected(Id leaderId, bool isHealthyCluster, bool isLocalNodeLeading) =>
            InformLeaderElectedCheck.IncrementAndGet();

        public void InformLeaderLost(Id lostLeaderId, bool isHealthyCluster) => InformLeaderLostCheck.IncrementAndGet();

        public void InformLocalNodeShutDown(Id nodeId) => InformLocalNodeShutDownCheck.IncrementAndGet();

        public void InformLocalNodeStarted(Id nodeId) => InformLocalNodeStartedCheck.IncrementAndGet();

        public void InformNodeIsHealthy(Id nodeId, bool isHealthyCluster) => InformNodeIsHealthyCheck.IncrementAndGet();

        public void InformNodeJoinedCluster(Id nodeId, bool isHealthyCluster) =>
            InformNodeJoinedClusterCheck.IncrementAndGet();

        public void InformNodeLeftCluster(Id nodeId, bool isHealthyCluster) =>
            InformNodeLeftClusterCheck.IncrementAndGet();

        public void InformQuorumAchieved() => InformQuorumAchievedCheck.IncrementAndGet();

        public void InformQuorumLost() => InformQuorumLostCheck.IncrementAndGet();

        public void InformAttributesClient(IAttributesProtocol client)
        {
            AttributesClient = client;
            InformAttributesClientCheck.IncrementAndGet();
        }

        public void InformAttributeSetCreated(string attributeSetName) =>
            InformAttributeSetCreatedCheck.IncrementAndGet();

        public void InformAttributeAdded(string attributeSetName, string attributeName) =>
            InformAttributeAddedCheck.IncrementAndGet();

        public void InformAttributeRemoved(string attributeSetName, string attributeName) =>
            InformAttributeRemovedCheck.IncrementAndGet();

        public void InformAttributeSetRemoved(string attributeSetName) =>
            InformAttributeSetRemovedCheck.IncrementAndGet();

        public void InformAttributeReplaced(string attributeSetName, string attributeName) =>
            InformAttributeReplacedCheck.IncrementAndGet();
    }
}