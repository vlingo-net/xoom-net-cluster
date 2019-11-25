// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model.Message;
using Vlingo.Actors;

namespace Vlingo.Cluster.Model.Node
{
    using Vlingo.Wire.Node;
    
    internal abstract class LiveNodeState
    {
        protected readonly ILiveNodeMaintainer LiveNodeMaintainer;
        protected readonly ILogger Logger;
        protected readonly Type StateType;
        protected readonly Node Node;
        
        internal TimeoutTracker NoQuorumTracker => new TimeoutTracker(Properties.Instance.ClusterQuorumTimeout());
        internal TimeoutTracker LeaderElectionTracker => new TimeoutTracker(Properties.Instance.ClusterHeartbeatInterval());

        internal LiveNodeState(
            Node node,
            ILiveNodeMaintainer liveNodeMaintainer,
            Type stateType,
            ILogger logger)
        {
            Node = node;
            LiveNodeMaintainer = liveNodeMaintainer;
            StateType = stateType;
            Logger = logger;
        }
        
        internal protected virtual void Handle(Directory dir) {
            Logger.Debug($"{StateType} {Node.Id} DIRECTORY: {dir}");
            LiveNodeMaintainer.MergeAllDirectoryEntries(dir.Nodes);
        }
        
        internal protected virtual void Handle(Elect elec) {
            Logger.Debug($"{StateType} {Node.Id} ELECT: {elec}");
            LiveNodeMaintainer.EscalateElection(elec.Id);
        }
        
        internal protected virtual void Handle(Join join) {
            Logger.Debug($"{StateType} {Node.Id} JOIN: {join}");
            LiveNodeMaintainer.JoinLocalWith(join.Node);
        }
        
        internal protected virtual void Handle(Leader leader) {
            Logger.Debug($"{StateType} {Node.Id} LEADER: {leader}");
            LiveNodeMaintainer.AssertNewLeadership(leader.Id);
        }
        
        internal protected virtual void Handle(Leave leave) {
            Logger.Debug($"{StateType} {Node.Id} LEAVE: {leave}");
            LiveNodeMaintainer.DropNode(leave.Id);
        }
        
        internal protected virtual void Handle(Ping ping) {
            Logger.Debug($"{StateType} {Node.Id} PING: {ping}");
            LiveNodeMaintainer.ProvidePulseTo(ping.Id);
        }
        
        internal protected virtual void Handle(Pulse pulse) {
            Logger.Debug($"{StateType} {Node.Id} PULSE: {pulse}");
            LiveNodeMaintainer.UpdateLastHealthIndication(pulse.Id);
        }
        
        internal protected virtual void Handle(Split split) {
            Logger.Debug($"{StateType} {Node.Id} SPLIT: {split}");
            LiveNodeMaintainer.DeclareNodeSplit(split.Id);
        }
        
        internal protected virtual void Handle(Vote vote) {
            Logger.Debug($"{StateType} {Node.Id} VOTE: {vote}");
            LiveNodeMaintainer.PlaceVote(vote.Id);
        }

        internal protected bool IsIdle => StateType == Type.Idle;

        internal protected bool IsFollower => StateType == Type.Follower;

        internal protected bool IsLeader => StateType == Type.Leader;

        public override string ToString() => $"{GetType().Name}[type={StateType} node={Node}]";

        internal enum Type {
            Idle,
            Follower,
            Leader
        }
    }
}