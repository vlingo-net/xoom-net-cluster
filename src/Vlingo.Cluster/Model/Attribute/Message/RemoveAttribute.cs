// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Cluster.Model.Attribute.Message
{
    public sealed class RemoveAttribute : AttributeMessage
    {
        public static RemoveAttribute From(Xoom.Wire.Node.Node node, AttributeSet set, TrackedAttribute tracked) => new RemoveAttribute(node, set, tracked);

        public RemoveAttribute(Xoom.Wire.Node.Node node, AttributeSet set, TrackedAttribute tracked) : base(node, set, tracked, ApplicationMessageType.RemoveAttribute)
        {
        }
    }
}