// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model.Attribute.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Attribute
{
    internal class RemoteAttributeRequestHandler
    {
        private readonly IConfiguration _configuration;
        private readonly ConfirmingDistributor _confirmingDistributor;
        private readonly AttributeSetRepository _repository;

        internal RemoteAttributeRequestHandler(
            ConfirmingDistributor confirmingDistributor,
            IConfiguration configuration,
            AttributeSetRepository repository)
        {
            _confirmingDistributor = confirmingDistributor;
            _configuration = configuration;
            _repository = repository;
        }

        internal void AddAttribute(ReceivedAttributeMessage request)
        {
            var attributeSet = _repository.AttributeSetOf(request.AttributeSetName!);
            if (attributeSet.IsNone)
            {
                attributeSet = AttributeSet.Named(request.AttributeSetName);
                _repository.Add(attributeSet);
            }
            var tracked = attributeSet.AddIfAbsent(request.Attribute());
            _confirmingDistributor.Confirm(request.TrackingId, attributeSet, tracked, request.Type, _configuration.NodeMatching(request.SourceNodeId));
        }
        
        internal void CreateAttributeSet(ReceivedAttributeMessage request)
        {
            var attributeSet = _repository.AttributeSetOf(request.AttributeSetName!);
            if (attributeSet.IsNone)
            {
                attributeSet = AttributeSet.Named(request.AttributeSetName);
                _repository.Add(attributeSet);
            }
            _confirmingDistributor.ConfirmCreate(request.TrackingId, attributeSet, _configuration.NodeMatching(request.SourceNodeId));
        }
        
        internal void RemoveAttributeSet(ReceivedAttributeMessage request)
        {
            var attributeSet = _repository.AttributeSetOf(request.AttributeSetName!);
            if (attributeSet.IsDefined)
            {
                attributeSet = AttributeSet.Named(request.AttributeSetName);
                _repository.Remove(request.AttributeSetName!);
            }
            _confirmingDistributor.ConfirmRemove(request.TrackingId, attributeSet, _configuration.NodeMatching(request.SourceNodeId));
        }
        
        internal void ReplaceAttribute(ReceivedAttributeMessage request)
        {
            var attributeSet = _repository.AttributeSetOf(request.AttributeSetName!);
            if (attributeSet.IsDefined)
            {
                var tracked = attributeSet.Replace(request.Attribute());
                if (tracked.IsPresent) // was both present and replaced
                {
                    _confirmingDistributor.Confirm(request.TrackingId, attributeSet, tracked, request.Type, _configuration.NodeMatching(request.SourceNodeId));
                }
            }
        }
        
        internal void RemoveAttribute(ReceivedAttributeMessage request)
        {
            var attributeSet = _repository.AttributeSetOf(request.AttributeSetName!);
            if (attributeSet.IsDefined)
            {
                var tracked = attributeSet.Remove(request.Attribute());
                if (tracked.IsPresent) // actually was present, now removed
                {
                    _confirmingDistributor.Confirm(request.TrackingId, attributeSet, tracked, request.Type, _configuration.NodeMatching(request.SourceNodeId));
                }
            }
        }
    }
}