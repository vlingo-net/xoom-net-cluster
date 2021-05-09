// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Nodes
{
    public interface ILiveNodeMaintainer : INodeSynchronizer
    {
        void AssertNewLeadership(Id id);
        
        void DeclareLeadership();
        
        void DeclareNodeSplit(Id leaderNodeId);
        
        void DropNode(Id id);
        
        void EscalateElection(Id id);
        
        void Join(Node node);
        
        void JoinLocalWith(Node remoteNode);
        
        void MergeAllDirectoryEntries(IEnumerable<Node> nodes);
        
        void OvertakeLeadership(Id leaderNodeId);
        
        void PlaceVote(Id voterId);
        
        void ProvidePulseTo(Id id);
        
        void UpdateLastHealthIndication(Id id);
        
        void VoteForLocalNode(Id targetNodeId);
    }
}