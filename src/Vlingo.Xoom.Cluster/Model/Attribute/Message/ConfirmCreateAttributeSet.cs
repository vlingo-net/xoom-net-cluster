// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Text;

namespace Vlingo.Xoom.Cluster.Model.Attribute.Message;

public sealed class ConfirmCreateAttributeSet : ApplicationMessage
{        
    public ConfirmCreateAttributeSet(string? correlatingMessageId, Xoom.Wire.Nodes.Node node, AttributeSet set)
        : base(
            correlatingMessageId,
            ApplicationMessageType.ConfirmCreateAttributeSet,
            TrackingIdFor(node, ApplicationMessageType.ConfirmCreateAttributeSet, set.Name))
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
            .Append(CorrelatingMessageId)
            .Append("\n")
            .Append(TrackingId)
            .Append("\n")
            .Append(Type.ToString())
            .Append("\n")
            .Append(AttributeSetName);
    
        return builder.ToString();
    }
}