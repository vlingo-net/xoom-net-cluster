// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace Vlingo.Cluster.Model.Node
{
    public interface IRegistryInterest
    {
        void InformAllLiveNodes(IEnumerable<Xoom.Wire.Node.Node> liveNodes,  bool isHealthyCluster);
        
        void InformConfirmedByLeader(Xoom.Wire.Node.Node node,  bool isHealthyCluster);
        
        void InformCurrentLeader(Xoom.Wire.Node.Node node,  bool isHealthyCluster);
        
        void InformMergedAllDirectoryEntries(IEnumerable<Xoom.Wire.Node.Node> liveNodes,  IEnumerable<MergeResult> mergeResults,  bool isHealthyCluster);
        
        void InformLeaderDemoted(Xoom.Wire.Node.Node node,  bool isHealthyCluster);
        
        void InformNodeIsHealthy(Xoom.Wire.Node.Node node,  bool isHealthyCluster);
        
        void InformNodeJoinedCluster(Xoom.Wire.Node.Node node,  bool isHealthyCluster);
        
        void InformNodeLeftCluster(Xoom.Wire.Node.Node node,  bool isHealthyCluster);
        
        void InformNodeTimedOut(Xoom.Wire.Node.Node node,  bool isHealthyCluster);
    }
}