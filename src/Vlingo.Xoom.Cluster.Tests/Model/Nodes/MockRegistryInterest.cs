// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Cluster.Model.Nodes;

namespace Vlingo.Xoom.Cluster.Tests.Model.Nodes
{
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
        
        public IEnumerable<Xoom.Wire.Nodes.Node> LiveNodes { get; private set; }
        
        public IEnumerable<MergeResult> MergeResults { get; private set; }

        public void InformAllLiveNodes(IEnumerable<Xoom.Wire.Nodes.Node> liveNodes, bool isHealthyCluster) => ++InformAllLiveNodesCheck;

        public void InformConfirmedByLeader(Xoom.Wire.Nodes.Node node, bool isHealthyCluster) => ++InformConfirmedByLeaderCheck;

        public void InformCurrentLeader(Xoom.Wire.Nodes.Node node, bool isHealthyCluster) => ++InformCurrentLeaderCheck;

        public void InformMergedAllDirectoryEntries(IEnumerable<Xoom.Wire.Nodes.Node> liveNodes, IEnumerable<MergeResult> mergeResults, bool isHealthyCluster)
        {
            LiveNodes = liveNodes;
            MergeResults = mergeResults;

            ++InformMergedAllDirectoryEntriesCheck;
        }

        public void InformLeaderDemoted(Xoom.Wire.Nodes.Node node, bool isHealthyCluster) => ++InformLeaderDemotedCheck;

        public void InformNodeIsHealthy(Xoom.Wire.Nodes.Node node, bool isHealthyCluster) => ++InformNodeIsHealthyCheck;

        public void InformNodeJoinedCluster(Xoom.Wire.Nodes.Node node, bool isHealthyCluster) => ++InformNodeJoinedClusterCheck;

        public void InformNodeLeftCluster(Xoom.Wire.Nodes.Node node, bool isHealthyCluster) => ++InformNodeLeftClusterCheck;

        public void InformNodeTimedOut(Xoom.Wire.Nodes.Node node, bool isHealthyCluster) => ++InformNodeTimedOutCheck;
    }
}