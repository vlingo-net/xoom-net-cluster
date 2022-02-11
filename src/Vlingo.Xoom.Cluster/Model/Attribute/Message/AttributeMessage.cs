// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Text;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Attribute.Message;

public abstract class AttributeMessage : ApplicationMessage
{
    public string? AttributeSetName { get; }
        
    public string? AttributeName { get; }
        
    public string? AttributeType { get; }
        
    public string? AttributeValue { get; }

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
            .Append(AttributeSetName)
            .Append("\n")
            .Append(AttributeName)
            .Append("\n")
            .Append(AttributeType)
            .Append("\n")
            .Append(AttributeValue);
    
        return builder.ToString();
    }

    protected AttributeMessage(Node node, AttributeSet set, TrackedAttribute tracked, ApplicationMessageType type)
        : this(NoCorrelatingMessageId, node, set, tracked, type)
    {
            
    }
        
    protected AttributeMessage(string? correlatingMessageId, Node node, AttributeSet set, TrackedAttribute tracked, ApplicationMessageType type)
        : base(correlatingMessageId, type, TrackingIdFor(node, type, tracked.Id))
    {
        AttributeSetName = set.Name;
        AttributeName = tracked.Attribute?.Name;
        AttributeType = tracked.Attribute?.Type.ToString();
        AttributeValue = tracked.Attribute?.ToStringValue();
    }
}