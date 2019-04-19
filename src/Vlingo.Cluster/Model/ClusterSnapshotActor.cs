// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Node;
using Vlingo.Wire.Fdx.Inbound;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model
{
    public class ClusterSnapshotActor : Actor, IClusterSnapshot, IClusterSnapshotControl, IInboundStreamInterest, IRegistryInterest
    {
        public void QuorumAchieved()
        {
            throw new System.NotImplementedException();
        }

        public void QuorumLost()
        {
            throw new System.NotImplementedException();
        }

        public void ShutDown()
        {
            throw new System.NotImplementedException();
        }

        public void HandleInboundStreamMessage(AddressType addressType, RawMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void InformAllLiveNodes(IEnumerable<Wire.Node.Node> liveNodes, bool isHealthyCluster)
        {
            throw new System.NotImplementedException();
        }

        public void InformConfirmedByLeader(Wire.Node.Node node, bool isHealthyCluster)
        {
            throw new System.NotImplementedException();
        }

        public void InformCurrentLeader(Wire.Node.Node node, bool isHealthyCluster)
        {
            throw new System.NotImplementedException();
        }

        public void InformMergedAllDirectoryEntries(IEnumerable<Wire.Node.Node> liveNodes, IEnumerable<MergeResult> mergeResults, bool isHealthyCluster)
        {
            throw new System.NotImplementedException();
        }

        public void InformLeaderDemoted(Wire.Node.Node node, bool isHealthyCluster)
        {
            throw new System.NotImplementedException();
        }

        public void InformNodeIsHealthy(Wire.Node.Node node, bool isHealthyCluster)
        {
            throw new System.NotImplementedException();
        }

        public void InformNodeJoinedCluster(Wire.Node.Node node, bool isHealthyCluster)
        {
            throw new System.NotImplementedException();
        }

        public void InformNodeLeftCluster(Wire.Node.Node node, bool isHealthyCluster)
        {
            throw new System.NotImplementedException();
        }

        public void InformNodeTimedOut(Wire.Node.Node node, bool isHealthyCluster)
        {
            throw new System.NotImplementedException();
        }
    }
}