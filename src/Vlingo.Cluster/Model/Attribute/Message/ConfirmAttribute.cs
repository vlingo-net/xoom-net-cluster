// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Cluster.Model.Attribute.Message
{
    using Vlingo.Wire.Node;
    
    public sealed class ConfirmAttribute : AttributeMessage
    {
        public static ConfirmAttribute From(
            string correlatingMessageId,
            Node node,
            AttributeSet set,
            TrackedAttribute tracked,
            ApplicationMessageType type) => new ConfirmAttribute(correlatingMessageId, node, set, tracked, type);

        public ConfirmAttribute(
            string correlatingMessageId,
            Node node,
            AttributeSet set,
            TrackedAttribute tracked,
            ApplicationMessageType type) : base(correlatingMessageId, node, set, tracked, type)
        {
        }
    }
}