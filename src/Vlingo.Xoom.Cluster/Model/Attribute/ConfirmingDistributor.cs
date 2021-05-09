// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Linq;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model.Application;
using Vlingo.Xoom.Cluster.Model.Attribute.Message;
using Vlingo.Xoom.Cluster.Model.Message;
using Vlingo.Xoom.Cluster.Model.Outbound;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Attribute
{
    public sealed class ConfirmingDistributor
    {
        private readonly IClusterApplication _application;
        private readonly Confirmables _confirmables;
  
        private readonly IEnumerable<Node> _allOtherNodes;
        private readonly ILogger _logger;
        private readonly Node _node;
        private readonly IOperationalOutboundStream _outbound;

        public void DistributeRemove(AttributeSet set) => DistributeRemoveTo(set, _allOtherNodes);

        internal ConfirmingDistributor(IClusterApplication application, Node node, IOperationalOutboundStream outbound, IConfiguration configuration)
        {
            _application = application;
            _node = node;
            _outbound = outbound;
            _logger = configuration.Logger;
            _allOtherNodes = configuration.AllOtherNodes(node.Id);
            _confirmables = new Confirmables(node, _allOtherNodes);

        }

        internal void AcknowledgeConfirmation(string? trackingId, Node node) => _confirmables.Confirm(trackingId, node);
        
        internal void DistributeCreate(AttributeSet set) => DistributeTo(set, _allOtherNodes);

        internal void DistributeTo(AttributeSet set, IEnumerable<Node> nodes)
        {
            var create = new CreateAttributeSet(_node, set);
            var confirmable = _confirmables.UnconfirmedFor(create, nodes);
            _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, create.ToPayload()), confirmable.UnconfirmedNodes);
            _application.InformAttributeSetCreated(set.Name);

            foreach (var tracked in set.All)
            {
                DistributeTo(set, tracked, ApplicationMessageType.AddAttribute, nodes);
            }
        }

        internal void DistributeRemoveTo(AttributeSet set, IEnumerable<Node> nodes)
        {
            // remove attributes first, then the set
            var allNodes = nodes as Node[] ?? nodes.ToArray();
            foreach (var untracked in set.All)
            {
                DistributeTo(set, untracked, ApplicationMessageType.RemoveAttribute, allNodes);
            }
            
            var removeSet = new RemoveAttributeSet(_node, set);
            var confirmable = _confirmables.UnconfirmedFor(removeSet, allNodes);
            _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, removeSet.ToPayload()), confirmable.UnconfirmedNodes);
            _application.InformAttributeSetRemoved(set.Name);
        }
        
        internal void Distribute(AttributeSet set, TrackedAttribute tracked, ApplicationMessageType type) =>
            DistributeTo(set, tracked, type, _allOtherNodes);

        internal void DistributeTo(
            AttributeSet set,
            TrackedAttribute tracked,
            ApplicationMessageType type,
            IEnumerable<Node> nodes)
        {
            switch (type)
            {
                case ApplicationMessageType.AddAttribute:
                    var add = AddAttribute.From(_node, set, tracked);
                    var addConfirmable = _confirmables.UnconfirmedFor(add, nodes);
                    _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, add.ToPayload()), addConfirmable.UnconfirmedNodes);
                    _application.InformAttributeAdded(set.Name!, tracked.Attribute?.Name);
                    break;
                case ApplicationMessageType.RemoveAttribute:
                    var remove = RemoveAttribute.From(_node, set, tracked);
                    var removeConfirmable = _confirmables.UnconfirmedFor(remove, nodes);
                    _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, remove.ToPayload()), removeConfirmable.UnconfirmedNodes);
                    _application.InformAttributeRemoved(set.Name!, tracked.Attribute?.Name);
                    break;
                case ApplicationMessageType.RemoveAttributeSet:
                    var removeSet = RemoveAttributeSet.From(_node, set);
                    var removeSetConfirmable = _confirmables.UnconfirmedFor(removeSet, nodes);
                    _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, removeSet.ToPayload()), removeSetConfirmable.UnconfirmedNodes);
                    _application.InformAttributeSetRemoved(set.Name);
                    break;
                case ApplicationMessageType.ReplaceAttribute:
                    var replace = ReplaceAttribute.From(_node, set, tracked);
                    var replaceConfirmable = _confirmables.UnconfirmedFor(replace, nodes);
                    _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, replace.ToPayload()), replaceConfirmable.UnconfirmedNodes);
                    _application.InformAttributeReplaced(set.Name!, tracked.Attribute?.Name);
                    break;
                default:
                    throw new InvalidOperationException("Cannot distribute unknown ApplicationMessageType.");
            }
        }

        internal void ConfirmCreate(string? correlatingMessageId, AttributeSet set, Node toOriginalSource)
        {
            var confirm = new ConfirmCreateAttributeSet(correlatingMessageId, _node, set);
            _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, confirm.ToPayload()), toOriginalSource.Collected);
            _application.InformAttributeSetCreated(set.Name);
        }
        
        internal void ConfirmRemove(string? correlatingMessageId, AttributeSet set, Node toOriginalSource)
        {
            var confirm = new ConfirmRemoveAttributeSet(correlatingMessageId, _node, set);
            _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, confirm.ToPayload()), toOriginalSource.Collected);
            _application.InformAttributeSetRemoved(set.Name);
        }

        internal void Confirm(
            string? correlatingMessageId,
            AttributeSet set,
            TrackedAttribute tracked,
            ApplicationMessageType type, Node toOriginalSource)
        {
            switch (type)
            {
                case ApplicationMessageType.AddAttribute:
                    var confirmAdd = ConfirmAttribute.From(correlatingMessageId, toOriginalSource, set, tracked, ApplicationMessageType.ConfirmAddAttribute);
                    _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, confirmAdd.ToPayload()), toOriginalSource.Collected);
                    _application.InformAttributeAdded(set.Name!, tracked.Attribute?.Name);
                    break;
                case ApplicationMessageType.RemoveAttribute:
                    var confirmRemove = ConfirmAttribute.From(correlatingMessageId, toOriginalSource, set, tracked, ApplicationMessageType.ConfirmRemoveAttribute);
                    _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, confirmRemove.ToPayload()), toOriginalSource.Collected);
                    _application.InformAttributeRemoved(set.Name!, tracked.Attribute?.Name);
                    break;
                case ApplicationMessageType.ReplaceAttribute:
                    var confirmReplace = ConfirmAttribute.From(correlatingMessageId, toOriginalSource, set, tracked, ApplicationMessageType.ConfirmReplaceAttribute);
                    _outbound.Application(ApplicationSays.From(_node.Id, _node.Name, confirmReplace.ToPayload()), toOriginalSource.Collected);
                    _application.InformAttributeReplaced(set.Name!, tracked.Attribute?.Name);
                    break;
                default:
                    throw new InvalidOperationException("Cannot confirm unknown ApplicationMessageType.");
            }
        }
        
        internal void RedistributeUnconfirmed()
        {
            foreach (var confirmable in _confirmables.AllRedistributable)
            {
                if (confirmable.HasUnconfirmedNodes)
                {
                    _logger.Trace($"REDIST ATTR: {confirmable}");
                    _outbound.Application(ApplicationSays.From(
                            _node.Id, _node.Name,
                            confirmable.Message?.ToPayload()),
                        confirmable.UnconfirmedNodes);
                }
            }
        }
        
        internal void SynchronizeTo(IEnumerable<AttributeSet> sets, Node targetNode)
        {
            var onlyOneTargetNode = targetNode.Collected;
            foreach (var set in sets)
            {
                DistributeTo(set, onlyOneTargetNode);
            }
        }

        internal IEnumerable<Node> UnconfirmedNodesFor(string trackingId) => _confirmables.ConfirmableOf(trackingId).UnconfirmedNodes;

        internal IEnumerable<string> AllTrackingIds => _confirmables.AllTrackingIds;
    }
}