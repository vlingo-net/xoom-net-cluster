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
    public interface IRegistry
    {
        void CleanTimedOutNodes();

        void ConfirmAllLiveNodesByLeader();

        bool IsConfirmedByLeader(Id id);

        void DeclareLeaderAs(Id id);

        void DemoteLeaderOf(Id id);

        bool IsLeader(Id id);

        bool HasMember(Id id);

        void Join(Node node);

        void Leave(Id id);

        void MergeAllDirectoryEntries(IEnumerable<Node> nodes);

        void PromoteElectedLeader(Id leaderNodeId);

        void RegisterRegistryInterest(IRegistryInterest interest);

        void UpdateLastHealthIndication(Id id);
        
        Node CurrentLeader { get; }
        
        bool HasLeader { get; }
        
        bool IsSingleNodeCluster { get; }
        
        IEnumerable<Node> LiveNodes { get; }
        
        bool HasQuorum { get; }
    }
}