// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model.Attribute.Message;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Attribute
{
    public class NoOpConfirmationInterest : IConfirmationInterest
    {
        private readonly ILogger _logger;

        public NoOpConfirmationInterest(IConfiguration configuration) => _logger = configuration.Logger;

        public void Confirm(Id confirmingNodeId, string? attributeSetName, string? attributeName, ApplicationMessageType type) => 
            _logger.Debug($"ATTR CONFIRMATION: NODE: {confirmingNodeId.Value} SET: {attributeSetName} ATTR: {attributeName} TYPE: {type}");
    }
}