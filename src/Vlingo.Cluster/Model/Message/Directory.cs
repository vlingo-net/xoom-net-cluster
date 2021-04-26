// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Cluster.Model.Message
{
    public sealed class Directory : OperationalMessage
    {
        private readonly Name _name;
        private readonly List<Node> _nodes;
        
        public static Directory From(string content)
        {
            var id = OperationalMessagePartsBuilder.IdFrom(content);
            var name = OperationalMessagePartsBuilder.NameFrom(content);
            var nodes = OperationalMessagePartsBuilder.NodesFrom(content);
            
            return new Directory(id, name, nodes);
        }

        public Directory(Id id, Name name, IEnumerable<Node> nodes) : base(id)
        {
            _name = name;
            _nodes = nodes
                .OrderBy(n => n.Id)
                .ThenBy(n => n.Name)
                .ThenBy(n => n.OperationalAddress)
                .ThenBy(n => n.ApplicationAddress)
                .ToList();
        }

        public override bool IsDirectory => true;

        public bool IsValid
        {
            get
            {
                foreach (var node in _nodes)
                {
                    if (!node.IsValid)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public IEnumerable<Node> Nodes => _nodes;
        
        public Name Name => _name;

        public int Count => _nodes.Count;

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Directory))
            {
                return false;
            }

            var otherDirectory = (Directory) obj;

            if (_nodes.Count != otherDirectory._nodes.Count)
            {
                return false;
            }

            for (var i = 0; i < _nodes.Count; i++)
            {
                if (!_nodes[i].Equals(otherDirectory._nodes[i]))
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
                    hashCode *= 31 * node.GetHashCode();
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
            builder.Append($"Directory[{Id},{Name},");

            foreach (var node in _nodes)
            {
                builder.AppendLine(node.ToString());
            }

            builder.Append("]");

            return builder.ToString();
        }
    }
}