// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Actors;

namespace Vlingo.Cluster.Model.Node
{
    using Vlingo.Wire.Node;
    
    public class RegistryInterestBroadcaster : IRegistryInterest
    {
        private readonly ILogger _logger;
        private readonly List<IRegistryInterest> _registryInterests;

        public RegistryInterestBroadcaster(ILogger logger)
        {
            _logger = logger;
            _registryInterests = new List<IRegistryInterest>();
        }

        public void RegisterRegistryInterest(IRegistryInterest interest) => _registryInterests.Add(interest);
        
        //========================================
        // RegistryInterest
        //========================================

        public void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster) =>
            Broadcast(interest => interest.InformAllLiveNodes(liveNodes, isHealthyCluster));

        public void InformConfirmedByLeader(Node node, bool isHealthyCluster) =>
            Broadcast(interest => interest.InformConfirmedByLeader(node, isHealthyCluster));

        public void InformCurrentLeader(Node node, bool isHealthyCluster) =>
            Broadcast(interest => interest.InformCurrentLeader(node, isHealthyCluster));

        public void InformMergedAllDirectoryEntries(
            IEnumerable<Node> liveNodes,
            IEnumerable<MergeResult> mergeResults,
            bool isHealthyCluster) => Broadcast(interest => interest.InformMergedAllDirectoryEntries(liveNodes, mergeResults, isHealthyCluster));

        public void InformLeaderDemoted(Node node, bool isHealthyCluster) =>
            Broadcast(interest => interest.InformLeaderDemoted(node, isHealthyCluster));

        public void InformNodeIsHealthy(Node node, bool isHealthyCluster) =>
            Broadcast(interest => interest.InformNodeIsHealthy(node, isHealthyCluster));

        public void InformNodeJoinedCluster(Node node, bool isHealthyCluster) =>
            Broadcast(interest => interest.InformNodeJoinedCluster(node, isHealthyCluster));

        public void InformNodeLeftCluster(Node node, bool isHealthyCluster) =>
            Broadcast(interest => interest.InformNodeLeftCluster(node, isHealthyCluster));

        public void InformNodeTimedOut(Node node, bool isHealthyCluster) =>
            Broadcast(interest => interest.InformNodeTimedOut(node, isHealthyCluster));
        
        private void Broadcast(Action<IRegistryInterest> inform) {
            foreach (var interest in _registryInterests)
            {
                try
                {
                    inform(interest);
                }
                catch (Exception e)
                {
                    _logger.Log($"Cannot inform because: {e.Message}", e);
                }   
            }
        }
    }
}