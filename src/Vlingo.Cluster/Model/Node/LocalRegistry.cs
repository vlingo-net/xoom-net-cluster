// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Actors;

namespace Vlingo.Cluster.Model.Node
{
    using Vlingo.Wire.Node;
    
    public sealed class LocalRegistry : IRegistry
    {
        private readonly Node _localNode;
        private readonly IConfiguration _configuration;
        private readonly Node localNode;
        private readonly ILogger _logger;
        private Dictionary<Id, RegisteredNodeStatus> _registry;

        public LocalRegistry(Node localNode, IConfiguration confirguration, ILogger logger)
        {
            _localNode = localNode;
        }
        
        public void CleanTimedOutNodes()
        {
            throw new System.NotImplementedException();
        }

        public void ConfirmAllLiveNodesByLeader()
        {
            throw new System.NotImplementedException();
        }

        public bool IsConfirmedByLeader(Id id)
        {
            throw new System.NotImplementedException();
        }

        public void DeclareLeaderAs(Id id)
        {
            throw new System.NotImplementedException();
        }

        public void DemoteLeaderOf(Id id)
        {
            throw new System.NotImplementedException();
        }

        public bool IsLeader(Id id)
        {
            throw new System.NotImplementedException();
        }

        public bool HasMember(Id id)
        {
            throw new System.NotImplementedException();
        }

        public void Join(Node node)
        {
            throw new System.NotImplementedException();
        }

        public void Leave(Id id)
        {
            throw new System.NotImplementedException();
        }

        public void MergeAllDirectoryEntries(IEnumerable<Node> nodes)
        {
            throw new System.NotImplementedException();
        }

        public void PromoteElectedLeader(Id leaderNodeId)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterRegistryInterest(IRegistryInterest interest)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateLastHealthIndication(Id id)
        {
            throw new System.NotImplementedException();
        }

        public Node CurrentLeader { get; }
        
        public bool HasLeader { get; }
        
        public IEnumerable<Node> LiveNodes { get; }
        
        public bool HasQuorum { get; }
    }
}