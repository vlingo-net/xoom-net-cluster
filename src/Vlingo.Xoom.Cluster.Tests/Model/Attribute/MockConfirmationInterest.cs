// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Cluster.Model.Attribute;
using Vlingo.Xoom.Cluster.Model.Attribute.Message;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Tests.Model.Attribute
{
    public class MockConfirmationInterest : IConfirmationInterest
    {
        public void Confirm(Id confirmingNodeId, string attributeSetName, string attributeName, ApplicationMessageType type)
        {
            NodeId = confirmingNodeId.Value;
            AttributeSetName = attributeSetName;
            AttributeName = attributeName;
            Type = type;
            ++Confirmed;
        }
        
        public short NodeId { get; private set; }
        
        public string AttributeName { get; private set; }
        
        public string AttributeSetName { get; private set; }
        
        public int Confirmed { get; private set; }
        
        public ApplicationMessageType Type { get; private set; }
    }
}