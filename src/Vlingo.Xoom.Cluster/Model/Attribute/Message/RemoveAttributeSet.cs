// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Text;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Attribute.Message
{
    public sealed class RemoveAttributeSet : ApplicationMessage
    {
        public static RemoveAttributeSet From(Node node, AttributeSet set) => new RemoveAttributeSet(node, set);
        
        public RemoveAttributeSet(Node node, AttributeSet set)
            : base(
                NoCorrelatingMessageId,
                ApplicationMessageType.RemoveAttributeSet,
                TrackingIdFor(node, ApplicationMessageType.RemoveAttributeSet, set.Name))
        {
            AttributeSetName = set.Name;
        }
        
        public string? AttributeSetName { get; }

        public override string ToPayload()
        {
            var builder = new StringBuilder();
            
            builder
                .Append(GetType().Name)
                .Append("\n")
                .Append(TrackingId)
                .Append("\n")
                .Append(Type.ToString())
                .Append("\n")
                .Append(AttributeSetName);
    
            return builder.ToString();
        }
    }
}