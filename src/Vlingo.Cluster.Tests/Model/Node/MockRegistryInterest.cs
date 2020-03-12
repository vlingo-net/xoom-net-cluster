// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Cluster.Model.Node;

namespace Vlingo.Cluster.Tests.Model.Node
{
    using Vlingo.Wire.Node;
    
    public class MockRegistryInterest : IRegistryInterest
    {
        public int InformAllLiveNodesCheck { get; private set; }
        
        public int InformConfirmedByLeaderCheck { get; private set; }
        
        public int InformCurrentLeaderCheck { get; private set; }
        
        public int InformMergedAllDirectoryEntriesCheck { get; private set; }
        
        public int InformLeaderDemotedCheck { get; private set; }
        
        public int InformNodeIsHealthyCheck { get; private set; }
        
        public int InformNodeJoinedClusterCheck { get; private set; }
        
        public int InformNodeLeftClusterCheck { get; private set; }
        
        public int InformNodeTimedOutCheck { get; private set; }
        
        public IEnumerable<Node> LiveNodes { get; private set; }
        
        public IEnumerable<MergeResult> MergeResults { get; private set; }

        public void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster) => ++InformAllLiveNodesCheck;

        public void InformConfirmedByLeader(Node node, bool isHealthyCluster) => ++InformConfirmedByLeaderCheck;

        public void InformCurrentLeader(Node node, bool isHealthyCluster) => ++InformCurrentLeaderCheck;

        public void InformMergedAllDirectoryEntries(IEnumerable<Node> liveNodes, IEnumerable<MergeResult> mergeResults, bool isHealthyCluster)
        {
            LiveNodes = liveNodes;
            MergeResults = mergeResults;

            ++InformMergedAllDirectoryEntriesCheck;
        }

        public void InformLeaderDemoted(Node node, bool isHealthyCluster) => ++InformLeaderDemotedCheck;

        public void InformNodeIsHealthy(Node node, bool isHealthyCluster) => ++InformNodeIsHealthyCheck;

        public void InformNodeJoinedCluster(Node node, bool isHealthyCluster) => ++InformNodeJoinedClusterCheck;

        public void InformNodeLeftCluster(Node node, bool isHealthyCluster) => ++InformNodeLeftClusterCheck;

        public void InformNodeTimedOut(Node node, bool isHealthyCluster) => ++InformNodeTimedOutCheck;
    }
}