// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model.Message;
using Vlingo.Xoom.Cluster.Model.Outbound;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Nodes
{
    public sealed class LocalLiveNodeActor : Actor, ILocalLiveNode, ILiveNodeMaintainer, IScheduled<object>
    {
        private readonly ICancellable _cancellable;
        private readonly CheckHealth _checkHealth;
        private readonly IConfiguration _configuration;
        private LiveNodeState? _state;
        private readonly Node _node;
        private readonly List<INodeSynchronizer> _nodeSynchronizers;
        private readonly IOperationalOutboundStream _outbound;
        private bool _quorumAchieved;
        private readonly ILocalLiveNode _selfLocalLiveNode;
        private readonly IClusterSnapshot _snapshot;
        private readonly IRegistry _registry;

        public LocalLiveNodeActor(
            Node node,
            IClusterSnapshot snapshot,
            IRegistry registry,
            IOperationalOutboundStream outbound,
            IConfiguration configuration)
        {
            _node = node;
            _snapshot = snapshot;
            _registry = registry;
            _outbound = outbound;
            _configuration = configuration;
            _nodeSynchronizers = new List<INodeSynchronizer>();
            _selfLocalLiveNode = SelfAs<ILocalLiveNode>();
            _checkHealth = new CheckHealth(_node.Id);
            _cancellable = ScheduleHealthCheck();

            StartNode();
        }

        //===================================
        // LocalLiveNode
        //===================================

        public void Handle(OperationalMessage message)
        {
            if (message.IsDirectory) _state?.Handle((Directory) message);
            else if (message.IsElect) _state?.Handle((Elect) message);
            else if (message.IsJoin) _state?.Handle((Join) message);
            else if (message.IsLeader) _state?.Handle((Leader) message);
            else if (message.IsLeave) _state?.Handle((Leave) message);
            else if (message.IsPing) _state?.Handle((Ping) message);
            else if (message.IsPulse) _state?.Handle((Pulse) message);
            else if (message.IsSplit) _state?.Handle((Split) message);
            else if (message.IsVote) _state?.Handle((Vote) message);
            else if (message.IsCheckHealth)
            {
                CheckHealth();
                InformHealth();
            }
        }

        public void RegisterNodeSynchronizer(INodeSynchronizer nodeSynchronizer) =>
            _nodeSynchronizers.Add(nodeSynchronizer);
        
        //================================================
        //== LiveNodeMaintainer
        //================================================

        #region LiveNodeMaintainer

        public void AssertNewLeadership(Id assertingNodeId)
        {
            //--------------------------------------------------------------------------------------------
            // -- Handles the following kinds of conditions:
            // --
            // -- Cluster is {1,2,3} and {3} is the leader so {1} and {2} are followers. A network
            // -- partition occurs between {2} and {3} and {2} wants to take over as leader because
            // -- it thinks that {3} died, but {1} and {3} can still see each other. So how does {1}
            // -- deal with the situation where it is already in a quorum with {1,3} but {2} can’t
            // -- see node {3} so it tells {1} that it wants to be leader. Of course this can also
            // -- happen if the network partition occurs before {3} originally declares itself as
            // -- the leader, but both situations can be dealt with in the same way.
            // -- 
            // -- This is a simple solution and may need a better one.
            //--------------------------------------------------------------------------------------------

            var currentLeader = _registry.CurrentLeader;

            if (currentLeader.IsLeaderOver(assertingNodeId))
            {
                _outbound.Split(assertingNodeId, currentLeader.Id);
            }
            else
            {
                DeclareFollower();
                PromoteElectedLeader(assertingNodeId);
            }
        }

        public void DeclareLeadership()
        {
            _outbound.Directory(_registry.LiveNodes);
            _outbound.Leader();
        }

        public void DeclareNodeSplit(Id leaderNodeId)
        {
            DeclareFollower();
            PromoteElectedLeader(leaderNodeId);
        }

        public void DropNode(Id id)
        {
            var droppedLeader = _registry.IsLeader(id);

            DropNodeFromCluster(id);

            if (droppedLeader)
            {
                _state?.LeaderElectionTracker.Start(true);
                _outbound.Elect(_configuration.AllGreaterNodes(_node.Id));
            }

            if (_state != null && _state.IsLeader)
            {
                DeclareLeadership();
            }
        }

        public void EscalateElection(Id electId)
        {
            _registry.Join(_node);
            _registry.Join(_configuration.NodeMatching(electId));

            if (_node.Id.GreaterThan(electId))
            {
                if (_state != null && !_state.LeaderElectionTracker.HasNotStarted)
                {
                    _state.LeaderElectionTracker.Start(true);
                    _outbound.Elect(_configuration.AllGreaterNodes(_node.Id));
                }
                else if (_state != null && _state.LeaderElectionTracker.HasTimedOut)
                {
                    DeclareLeadership();
                    return;
                }

                _outbound.Vote(electId);
            }
        }

        public void Join(Node joiningNode)
        {
            _registry.Join(joiningNode);
            _outbound.Open(joiningNode.Id);
    
            if (_state != null && _state.IsLeader)
            {
                DeclareLeadership();
            }

            Synchronize(joiningNode);
        }

        public void JoinLocalWith(Node remoteNode)
        {
            Join(_node);
            Join(remoteNode);
        }

        public void MergeAllDirectoryEntries(IEnumerable<Node> nodes) => _registry.MergeAllDirectoryEntries(nodes); 

        public void OvertakeLeadership(Id leaderNodeId) => DeclareFollower();

        public void PlaceVote(Id voterId)
        {
            // should not happen that nodeId > voterId, unless
            // there is a late Join or Directory received
            if (_node.Id.GreaterThan(voterId))
            {
                _outbound.Vote(voterId);
            }
            else
            {
                _state?.LeaderElectionTracker.Clear();
            }
        }

        public void ProvidePulseTo(Id id) => _outbound.Pulse(id);
        
        public void Synchronize(Node node)
        {
            foreach (var syncher in _nodeSynchronizers)
            {
                syncher.Synchronize(node);
            }
        }

        public void UpdateLastHealthIndication(Id id) => _registry.UpdateLastHealthIndication(id);

        public void VoteForLocalNode(Id targetNodeId)
        {
            _outbound.Vote(targetNodeId);
            DeclareLeadership();
        }

        #endregion
        
        //===================================
        // Scheduled
        //===================================

        public void IntervalSignal(IScheduled<object> scheduled, object data)
        {
            _registry.CleanTimedOutNodes();
    
            _selfLocalLiveNode.Handle(_checkHealth);
        }
        
        //===================================
        // Stoppable
        //===================================

        public override void Stop()
        {
            _outbound.Leave();
            _cancellable.Cancel();
            _registry.Leave(_node.Id);
            base.Stop();
        }
        
        //===================================
        // internal implementation
        //===================================

        #region internal implementation

        private void CheckHealth()
        {
            if (_registry.HasQuorum)
            {
                MaintainHealthWithQuorum();
            }
            else
            {
                MaintainHealthWithNoQuorum();
            }
        }

        private void DeclareFollower()
        {
            if (_state == null || !_state.IsIdle)
            {
                Logger.Info($"Cluster follower: {_node}");

                _state = new FollowerState(_node, this, Logger);
            }
        }

        private void DeclareIdle()
        {
            if (_state == null || !_state.IsIdle)
            {
                Logger.Info($"Cluster idle: {_node}");

                _state = new IdleState(_node, this, Logger);
      
                if (_registry.CurrentLeader.Equals(_node))
                {
                    _registry.DemoteLeaderOf(_node.Id);
                }
            }
        }

        private void DeclareLeader()
        {
            Logger.Info($"Cluster leader: {_node}");

            _state = new LeaderState(_node, this, Logger);

            PromoteElectedLeader(_node.Id);

            _outbound.Directory(_registry.LiveNodes);

            _outbound.Leader();
        }

        private void DropNodeFromCluster(Id nodeId)
        {
            if (_registry.HasMember(nodeId))
            {
                _registry.Leave(nodeId);
                _outbound.Close(nodeId);
            }
        }

        private void InformHealth()
        {
            _outbound.Pulse();
            
            if (_registry.HasMember(_node.Id))
            {
                _registry.UpdateLastHealthIndication(_node.Id);
            }

            if (_state != null && (_state.IsIdle || !_registry.IsConfirmedByLeader(_node.Id)))
            {
                _outbound.Join();
            }
        }

        private void MaintainHealthWithNoQuorum()
        {
            _state?.LeaderElectionTracker.Reset();

            _state?.NoQuorumTracker.Start();

            WatchForQuorumRelinquished();

            if (_state != null && _state.NoQuorumTracker.HasTimedOut)
            {
                Logger.Warn("No quorum; leaving cluster to become idle node.");
                _registry.Leave(_node.Id);
                DeclareIdle();
            }
        }

        private void MaintainHealthWithQuorum()
        {
            _state?.NoQuorumTracker.Reset();

            WatchForQuorumAchievement();

            if (!_registry.HasLeader)
            {
                if (_state != null && !_state.LeaderElectionTracker.HasNotStarted)
                {
                    _state.LeaderElectionTracker.Start();
                    _outbound.Elect(_configuration.AllGreaterNodes(_node.Id));
                }
                else if (_state != null && _state.LeaderElectionTracker.HasTimedOut)
                {
                    DeclareLeader();
                }
            }
        }

        private void PromoteElectedLeader(Id leaderNodeId)
        {
            if (_node.Id.Equals(leaderNodeId))
            {
                // I've seen the leader get bumped out of its own
                // registry during a weird network partition or
                // something and it can never get back leadership
                // or even rejoin the cluster because it's missing
                // from the local registry
                _registry.Join(_node);

                _registry.DeclareLeaderAs(leaderNodeId);

                _registry.ConfirmAllLiveNodesByLeader();
            }
            else
            {
                if (_registry.IsLeader(_node.Id))
                {
                    _registry.DemoteLeaderOf(_node.Id);
                }
      
                if (!_registry.HasMember(leaderNodeId))
                {
                    _registry.Join(_configuration.NodeMatching(leaderNodeId));
                }
      
                _registry.DeclareLeaderAs(leaderNodeId);
            }
        }

        private ICancellable ScheduleHealthCheck()
        {
            return Stage.Scheduler.Schedule(SelfAs<IScheduled<object?>>(), null, TimeSpan.FromMilliseconds(1000L),
                TimeSpan.FromMilliseconds(Properties.Instance.ClusterHealthCheckInterval()));
        }
        
        private void StartNode()
        {
            if (_registry.IsSingleNodeCluster)
            {
                DeclareLeader();
            }
            else
            {
                DeclareIdle();
            }
        }

        private void WatchForQuorumAchievement()
        {
            if (!_quorumAchieved)
            {
                _quorumAchieved = true;
                _snapshot.QuorumAchieved();
            }
        }

        private void WatchForQuorumRelinquished()
        {
            if (_quorumAchieved)
            {
                _quorumAchieved = false;
                _snapshot.QuorumLost();
            }
        }

        #endregion
    }
}