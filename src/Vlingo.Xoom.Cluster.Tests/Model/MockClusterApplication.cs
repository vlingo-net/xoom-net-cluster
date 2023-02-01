// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Cluster.Model.Application;
using Vlingo.Xoom.Cluster.Model.Attribute;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Wire.Fdx.Outbound;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Tests.Model
{   
    public class MockClusterApplication : IClusterApplication
    {
        public MockClusterApplication()
        {
            AllLiveNodes = new AtomicInteger(0);
            HandleApplicationMessageCheck = new AtomicInteger(0);
            InformLeaderElectedCheck = new AtomicInteger(0);
            InformLeaderLostCheck = new AtomicInteger(0);
            InformLocalNodeShutDownCheck = new AtomicInteger(0);
            InformLocalNodeStartedCheck = new AtomicInteger(0);
            InformNodeIsHealthyCheck = new AtomicInteger(0);
            InformNodeJoinedClusterCheck = new AtomicInteger(0);
            InformNodeLeftClusterCheck = new AtomicInteger(0);
            InformQuorumAchievedCheck = new AtomicInteger(0);
            InformQuorumLostCheck = new AtomicInteger(0);
            InformResponderCheck = new AtomicInteger(0);
            InformAttributesClientCheck = new AtomicInteger(0);
            InformAttributeSetCreatedCheck = new AtomicInteger(0);
            InformAttributeAddedCheck = new AtomicInteger(0);
            InformAttributeRemovedCheck = new AtomicInteger(0);
            InformAttributeReplacedCheck = new AtomicInteger(0);
            InformAttributeSetRemovedCheck = new AtomicInteger(0);
            StopCheck = new AtomicInteger(0);
        }
        
        public IAttributesProtocol AttributesClient { get; private set; }
  
        public AtomicInteger AllLiveNodes { get; }
        
        public AtomicInteger HandleApplicationMessageCheck { get; }
  
        public AtomicInteger InformLeaderElectedCheck { get; }
        
        public AtomicInteger InformLeaderLostCheck { get; }
        
        public AtomicInteger InformLocalNodeShutDownCheck { get; }
        
        public AtomicInteger InformLocalNodeStartedCheck { get; }
        
        public AtomicInteger InformNodeIsHealthyCheck { get; }
        
        public AtomicInteger InformNodeJoinedClusterCheck { get; }
        
        public AtomicInteger InformNodeLeftClusterCheck { get; }
        
        public AtomicInteger InformQuorumAchievedCheck { get; }
        
        public AtomicInteger InformQuorumLostCheck { get; }
        
        public AtomicInteger InformResponderCheck { get; }
  
        public AtomicInteger InformAttributesClientCheck { get; }
        
        public AtomicInteger InformAttributeSetCreatedCheck { get; }
        
        public AtomicInteger InformAttributeAddedCheck { get; }
        
        public AtomicInteger InformAttributeRemovedCheck { get; }
        
        public AtomicInteger InformAttributeReplacedCheck { get; }
        
        public AtomicInteger InformAttributeSetRemovedCheck { get; }
  
        public AtomicInteger StopCheck { get; }
        
        public void Start()
        {
        }
        
        public void Conclude()
        {
            Stop();
        }

        public void Stop() => StopCheck.IncrementAndGet();

        public bool IsStopped => false;

        public void HandleApplicationMessage(RawMessage message) =>
            HandleApplicationMessageCheck.IncrementAndGet();

        public void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster) =>
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
        
        public void InformResponder(IApplicationOutboundStream responder) => InformResponderCheck.IncrementAndGet();

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