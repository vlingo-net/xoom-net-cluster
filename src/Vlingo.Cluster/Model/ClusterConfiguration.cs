// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Cluster.Model
{
    public class ClusterConfiguration : IConfiguration
    {
        private readonly ILogger _logger;
        private readonly IList<Node> _nodes;

        public ClusterConfiguration(ILogger logger)
        {
            _logger = logger;
            _nodes = new List<Node>();

            InitializeConfiguredNodeEntries(Properties.Instance);
        }
        
        public IEnumerable<Node> AllNodesOf(IEnumerable<Id> ids)
        {
            throw new System.NotImplementedException("Currently not used.");
        }

        public IEnumerable<Node> AllGreaterNodes(Id nodeId)
        {
            var greater = new List<Node>();
            foreach (var node in _nodes)
            {
                if (node.Id.GreaterThan(nodeId))
                {
                    greater.Add(node);
                }
            }
            
            return greater;
        }

        public IEnumerable<Node> AllOtherNodes(Id nodeId)
        {
            var except = new List<Node>();
            foreach (var node in _nodes)
            {
                if (!node.Id.Equals(nodeId))
                {
                    except.Add(node);
                }
            }

            return except;
        }

        public IEnumerable<Id> AllOtherNodesId(Id nodeId)
        {
            var ids = new List<Id>();
            foreach (var node in AllOtherNodes(nodeId))
            {
                ids.Add(node.Id);
            }

            return ids;
        }

        public Node NodeMatching(Id nodeId) => _nodes.SingleOrDefault(n => n.Id.Equals(nodeId)) ?? Node.NoNode;

        public bool HasNode(Id nodeId) => _nodes.Any(n => n.Id.Equals(nodeId));

        public IEnumerable<Node> AllNodes => _nodes;

        public IEnumerable<string> AllNodeNames => _nodes.Select(n => n.Name.Value);

        public Id GreatestNodeId => _nodes.Max(n => n.Id);

        public int TotalNodes => _nodes.Count;

        public ILogger Logger => _logger;

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(ClusterConfiguration))
            {
                return false;
            }

            var otherClusterConfiguration = (ClusterConfiguration) obj;

            if (_nodes.Count != otherClusterConfiguration._nodes.Count)
            {
                return false;
            }

            for (var i = 0; i < _nodes.Count; i++)
            {
                if (!_nodes[i].Id.Equals(otherClusterConfiguration._nodes[i].Id))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;

            foreach (var node in _nodes)
            {
                if (hashCode == 0)
                {
                    hashCode = 31 * node.GetHashCode();
                }
                else
                {
                    hashCode += node.GetHashCode();
                }
            }

            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var node in _nodes)
            {
                builder.AppendLine(node.ToString());
            }

            return $"ConfiguredCluster[{builder}]";
        }

        internal ClusterConfiguration(Properties properties, ILogger logger)
        {
            _logger = logger;
            _nodes = new List<Node>();
            
            InitializeConfiguredNodeEntries(properties);
        }

        private void InitializeConfiguredNodeEntries(Properties properties)
        {
            foreach (var configuredNodeName in Properties.Instance.SeedNodes())
            {
                var nodeId = Id.Of(properties.NodeId(configuredNodeName));
                var nodeName = new Name(configuredNodeName);
                var host = Host.Of(properties.Host(configuredNodeName));
                var opNodeAddress = Address.From(host, properties.OperationalPort(configuredNodeName), AddressType.Op);
                var appNodeAddress = Address.From(host, properties.ApplicationPort(configuredNodeName), AddressType.App);
                
                _nodes.Add(new Node(nodeId, nodeName, opNodeAddress, appNodeAddress));
            }
        }
    }
}