// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Cluster.Model.Nodes
{
    public sealed class LocalRegistry : IRegistry
    {
        private readonly RegistryInterestBroadcaster _broadcaster;
        private readonly Node _localNode;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private SortedDictionary<Id, RegisteredNodeStatus> _registry;

        public LocalRegistry(Node localNode, IConfiguration confirguration, ILogger logger)
        {
            _localNode = localNode;
            _configuration = confirguration;
            _logger = logger;
            _broadcaster = new RegistryInterestBroadcaster(logger);
            _registry = new SortedDictionary<Id, RegisteredNodeStatus>();
        }

        //======================================
        // Registry
        //======================================

        public void CleanTimedOutNodes()
        {
            var currentTime = DateTimeHelper.CurrentTimeMillis();
            var liveNodeTimeout = Properties.Instance.ClusterLiveNodeTimeout();

            var nodesToKeep = new SortedDictionary<Id, RegisteredNodeStatus>();

            foreach (var status in _registry.Values)
            {
                if (!status.IsTimedOut(currentTime, liveNodeTimeout))
                {
                    nodesToKeep.Add(status.Node.Id, status);
                }
                else
                {
                    DemoteLeaderOf(status.Node.Id);
                    _broadcaster.InformNodeTimedOut(status.Node, IsClusterHealthy());
                    _logger.Info($"Node cleaned from registry due to timeout: {status.Node}");
                }
            }
            
            _registry = nodesToKeep;
        }

        public void ConfirmAllLiveNodesByLeader()
        {
            foreach (var status in _registry.Values)
            {
                status.ConfirmedByLeader(true);
                _broadcaster.InformConfirmedByLeader(status.Node, IsClusterHealthy());
            }
        }

        public bool IsConfirmedByLeader(Id id)
        {
            if (_registry.TryGetValue(id, out var status))
            {
                return status.IsConfirmedByLeader;
            }
            
            return false;
        }

        public void DeclareLeaderAs(Id id)
        {
            if (_registry.TryGetValue(id, out var status))
            {
                status.Lead(true);
                status.UpdateLastHealthIndication();
                _broadcaster.InformCurrentLeader(status.Node, IsClusterHealthy());
                DemotePreviousLeader(id);
            }
            else
            {
                _logger.Warn($"Cannot declare leader because missing node: '{id}'");
            }
        }

        public void DemoteLeaderOf(Id id)
        {
            if (_registry.TryGetValue(id, out var status) && status.IsLeader)
            {
                status.Lead(false);
                _broadcaster.InformLeaderDemoted(status.Node, IsClusterHealthy());
            }
        }

        public bool IsLeader(Id id)
        {
            if (_registry.TryGetValue(id, out var status))
            {
                return status.IsLeader;
            }

            return false;
        }

        public bool HasMember(Id id) => _registry.ContainsKey(id);

        public void Join(Node node)
        {
            if (!HasMember(node.Id))
            {
                _registry.Add(node.Id, new RegisteredNodeStatus(node, false, false));
                _broadcaster.InformNodeJoinedCluster(node, IsClusterHealthy());
                _broadcaster.InformAllLiveNodes(LiveNodes, IsClusterHealthy());
            }
        }

        public void Leave(Id id)
        {
            if (_registry.TryGetValue(id, out var status))
            {
                _registry.Remove(id);
                DemoteLeaderOf(id);
                _broadcaster.InformNodeLeftCluster(status.Node, IsClusterHealthy());
                _broadcaster.InformAllLiveNodes(LiveNodes, IsClusterHealthy());
            }
            else
            {
                _logger.Warn($"Cannot leave because missing node: '{id}'");
            }
        }

        public void MergeAllDirectoryEntries(IEnumerable<Node> leaderRegisteredNodes)
        {
            var result = new SortedSet<MergeResult>();
            var mergedNodes = new SortedDictionary<Id, RegisteredNodeStatus>();

            foreach (var node in leaderRegisteredNodes)
            {
                mergedNodes.Add(node.Id, new RegisteredNodeStatus(node, IsLeader(node.Id), true));
            }

            foreach (var status in mergedNodes.Values)
            {
                if (!_registry.ContainsKey(status.Node.Id))
                {
                    result.Add(new MergeResult(status.Node, true));
                }
            }

            foreach (var status in _registry.Values)
            {
                if (!mergedNodes.ContainsKey(status.Node.Id))
                {
                    DemoteLeaderOf(status.Node.Id);
                    result.Add(new MergeResult(status.Node, false));
                }
            }

            _registry = mergedNodes;

            _broadcaster.InformMergedAllDirectoryEntries(LiveNodes, result, IsClusterHealthy());
            _broadcaster.InformAllLiveNodes(LiveNodes, IsClusterHealthy());
        }

        public void PromoteElectedLeader(Id leaderNodeId)
        {
            if (_localNode.Id.Equals(leaderNodeId))
            {
                DeclareLeaderAs(leaderNodeId);

                ConfirmAllLiveNodesByLeader();
            }
            else
            {
      
                if (IsLeader(_localNode.Id))
                {
                    DemoteLeaderOf(_localNode.Id);
                }
      
                if (!HasMember(leaderNodeId))
                {
                    Join(_configuration.NodeMatching(leaderNodeId));
                }
      
                DeclareLeaderAs(leaderNodeId);
            }

            _broadcaster.InformCurrentLeader(_registry[leaderNodeId].Node, IsClusterHealthy());
        }

        public void RegisterRegistryInterest(IRegistryInterest interest) =>
            _broadcaster.RegisterRegistryInterest(interest);

        public void UpdateLastHealthIndication(Id id)
        {
            if (_registry.TryGetValue(id, out var status))
            {
                status.UpdateLastHealthIndication();
                _broadcaster.InformNodeIsHealthy(status.Node, IsClusterHealthy());
            }
        }

        public Node CurrentLeader
        {
            get
            {
                foreach (var status in _registry.Values)
                {
                    if (status.IsLeader)
                    {
                        return status.Node;
                    }
                }

                return Node.NoNode;
            }
        }

        public bool HasLeader
        {
            get
            {
                foreach (var status in _registry.Values)
                {
                    if (status.IsLeader)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsSingleNodeCluster => _configuration.TotalNodes == 1;

        public IEnumerable<Node> LiveNodes => _registry.Values.Select(s => s.Node);

        public bool HasQuorum
        {
            get
            {
                var quorum = _configuration.TotalNodes / 2 + 1;
                return LiveNodes.Count() >= quorum;
            }
        }
        
        internal RegisteredNodeStatus RegisteredNodeStatusOf(Id id) => _registry[id];

        private bool IsClusterHealthy()
        {
            return HasQuorum && HasLeader;
        }

        private void DemotePreviousLeader(Id currentLeaderNodeId)
        {
            foreach (var status in _registry.Values)
            {
                if (!status.Node.Id.Equals(currentLeaderNodeId) && status.IsLeader)
                {
                    status.Lead(false);
                    _broadcaster.InformLeaderDemoted(status.Node, IsClusterHealthy());
                }   
            }
        }
    }
}