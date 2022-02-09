// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vlingo.Xoom.Cluster.Model.Attribute.Message;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Attribute
{
    internal sealed class Confirmables
    {
        private readonly List<Node> _allOtherNodes;
        private readonly List<Confirmable> _expectedConfirmables;
        private readonly Node _node;

        internal Confirmables(Node node, IEnumerable<Node> allOtherNodes)
        {
            _node = node;
            _allOtherNodes = new List<Node>(allOtherNodes);
            _expectedConfirmables = new List<Confirmable>();
        }

        internal void Confirm(string? trackingId, Node node)
        {
            var confirmable = ConfirmableOf(trackingId);
            confirmable.Confirm(node);
            if (!confirmable.HasUnconfirmedNodes)
            {
                _expectedConfirmables.Remove(confirmable);
            }
        }

        internal Confirmable ConfirmableOf(string? trackingId)
        {
            foreach (var confirmable in _expectedConfirmables)
            {
                if (confirmable.TrackingId.Equals(trackingId))
                {
                    return confirmable;
                }
            }
            
            return Confirmable.NoConfirmable;
        }

        internal Confirmable Unconfirmed(ApplicationMessage message) => UnconfirmedFor(message, _allOtherNodes);

        internal Confirmable UnconfirmedFor(ApplicationMessage message, IEnumerable<Node> nodes)
        {
            var allOtherNodes = nodes as Node[] ?? nodes.ToArray();
            if (allOtherNodes.Contains(_node))
            {
                throw new Exception();
            }
            
            var confirmable = new Confirmable(message, allOtherNodes);
            _expectedConfirmables.Add(confirmable);
            return confirmable;
        }

        internal IEnumerable<Confirmable> AllRedistributable =>
            _expectedConfirmables.Where(c => c.IsRedistributableAsOf());

        internal IEnumerable<string> AllTrackingIds => _expectedConfirmables.Select(c => c.Message!.TrackingId);

        internal sealed class Confirmable
        {
            internal static readonly int TotalRetries = Properties.Instance.ClusterAttributesRedistributionRetries();
            internal static readonly Confirmable NoConfirmable = new Confirmable();
    
            private readonly long _createdOn;
            private readonly ApplicationMessage? _message;
            private readonly string _trackingId;
            private Dictionary<Node, int> _unconfirmedNodes;
            
            internal Confirmable(ApplicationMessage message, IEnumerable<Node> allOtherNodes)
            {
                _message = message;
                _unconfirmedNodes = AllUnconfirmedFor(allOtherNodes);
                _createdOn = DateTimeHelper.CurrentTimeMillis();
                _trackingId = message.TrackingId;
            }

            internal bool IsRedistributableAsOf()
            {
                var targetTime = _createdOn + Properties.Instance.ClusterAttributesRedistributionInterval();
                
                if (targetTime < DateTimeHelper.CurrentTimeMillis())
                {
                    var allUnconfirmed = new Dictionary<Node, int>(_unconfirmedNodes.Count);

                    foreach (var node in _unconfirmedNodes.Keys)
                    {
                        var tries = _unconfirmedNodes[node] + 1;
                        if (tries <= TotalRetries)
                        {
                            allUnconfirmed.Add(node, tries);
                        }
                    }
        
                    _unconfirmedNodes = allUnconfirmed;
        
                    return true;
                }
                
                return false;
            }

            internal void Confirm(Node node) => _unconfirmedNodes.Remove(node);

            internal bool HasUnconfirmedNodes => _unconfirmedNodes.Any();

            internal ApplicationMessage? Message => _message;

            internal IEnumerable<Node> UnconfirmedNodes => _unconfirmedNodes.Keys;

            internal string TrackingId => _trackingId;

            public override bool Equals(object? obj)
            {
                if (obj == null || obj.GetType() != typeof(Confirmable))
                {
                    return false;
                }

                return _trackingId.Equals(((Confirmable) obj)._trackingId);
            }

            public override int GetHashCode() => 31 * _trackingId.GetHashCode();

            public override string ToString()
            {
                var builder = new StringBuilder();
                builder.Append($"Confirmable[trackingId={_trackingId} nodes=");

                foreach (var unconfirmedNode in _unconfirmedNodes)
                {
                    builder.AppendLine($"{unconfirmedNode.Key}, {unconfirmedNode.Value}");
                }

                builder.Append("]");

                return builder.ToString();
            }

            private Confirmable()
            {
                _message = null;
                _unconfirmedNodes = new Dictionary<Node, int>();
                _createdOn = 0L;
                _trackingId = "";
            }
            
            private Dictionary<Node, int> AllUnconfirmedFor(IEnumerable<Node> allOtherNodes)
            {
                var otherNodes = allOtherNodes as Node[] ?? allOtherNodes.ToArray();
                var allUnconfirmed = new Dictionary<Node, int>(otherNodes.Count());
                foreach (var node in otherNodes)
                {
                    allUnconfirmed.Add(node, 0);
                }
                
                return allUnconfirmed;
            }
        }
    }
}