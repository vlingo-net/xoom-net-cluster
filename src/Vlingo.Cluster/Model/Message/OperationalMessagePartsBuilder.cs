// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Wire.Message;

namespace Vlingo.Cluster.Model.Message
{
    using Vlingo.Wire.Node;
    
    internal class OperationalMessagePartsBuilder
    {
        internal static string PayloadFrom(string content)
        {
            var index1 = content.IndexOf('\n');
            if (index1 == -1)
            {
                return string.Empty;
            }
            
            var index2 = content.IndexOf('\n', index1 + 1);
            
            if (index2 == -1)
            {
                return string.Empty;
            }
            
            var payload = content.Substring(index2 + 1);

            return payload;
        }

        internal static string? SaysIdFrom(string content)
        {
            var parts = content.Split('\n');
            
            if (parts.Length < 3)
            {
                return string.Empty;
            }
            
            var saysId = MessagePartsBuilder.ParseField(parts[2], "si=");

            return saysId;
        }

        internal static IEnumerable<Node> NodesFrom(string content) => MessagePartsBuilder.NodesFrom(content);

        internal static Node NodeFrom(string content) => MessagePartsBuilder.NodeFrom(content);

        internal static Id IdFrom(string content) => MessagePartsBuilder.IdFrom(content);

        internal static Name NameFrom(string content) => MessagePartsBuilder.NameFrom(content);
    }
}