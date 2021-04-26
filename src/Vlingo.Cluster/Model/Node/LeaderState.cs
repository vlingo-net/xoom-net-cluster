// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model.Message;
using Vlingo.Xoom.Actors;

namespace Vlingo.Cluster.Model.Node
{
    internal sealed class LeaderState : LiveNodeState
    {
        internal LeaderState(Xoom.Wire.Node.Node node, ILiveNodeMaintainer liveNodeMaintainer, ILogger logger) : base(node, liveNodeMaintainer, Type.Leader, logger)
        {
        }

        protected internal override void Handle(Directory dir)
        {
            Logger.Debug($"{StateType} {Node.Id} DIRECTORY: {dir}");

            if (dir.Id.GreaterThan(Node.Id))
            {
                // apparently a new bully is taking leadership --
                // perhaps there was a race for leadership on newly
                // joined node with higher nodeId

                LiveNodeMaintainer.MergeAllDirectoryEntries(dir.Nodes);
            }
            else
            {
                Logger.Warn($"Leader must not receive Directory message from follower: '{dir.Id}'");
            }
        }

        protected internal override void Handle(Elect elec)
        {
            Logger.Debug($"{StateType} {Node.Id} ELECT: {elec}");
            LiveNodeMaintainer.VoteForLocalNode(elec.Id);
        }

        protected internal override void Handle(Join join)
        {
            Logger.Debug($"{StateType} {Node.Id} JOIN: {join}");
            LiveNodeMaintainer.Join(join.Node);
        }

        protected internal override void Handle(Leader leader)
        {
            Logger.Debug($"{StateType} {Node.Id} LEADER: {leader}");
            if (leader.Id.Equals(Node.Id))
            {
                Logger.Warn("Leader must not receive Leader message of itself from a follower.");
            }
            else if (leader.Id.GreaterThan(Node.Id))
            {
                LiveNodeMaintainer.OvertakeLeadership(leader.Id);
            }
            else
            {
                LiveNodeMaintainer.DeclareLeadership();
            }
        }

        protected internal override void Handle(Leave leave)
        {
            Logger.Debug($"{StateType} {Node.Id} LEAVE: {leave}");
            if (leave.Id.Equals(Node.Id))
            {
                Logger.Warn("Leader must not receive Leave message of itself from a follower.");
            }
            else
            {
                LiveNodeMaintainer.DropNode(leave.Id);
            }
        }

        protected internal override void Handle(Vote vote)
        {
            Logger.Debug($"{StateType} {Node.Id} VOTE: {vote}");
            LiveNodeMaintainer.DeclareLeadership();
        }
    }
}