// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Message
{
    public sealed class Leave : OperationalMessage
    {
        public static Leave From(string content) =>  new Leave(OperationalMessagePartsBuilder.IdFrom(content));
        
        public Leave(Id id) : base(id)
        {
        }

        public override bool IsLeave => true;
        
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Leave))
            {
                return false;
            }

            return Id.Equals(((Leave) obj).Id);
        }

        public override int GetHashCode() => 31 * Id.GetHashCode();

        public override string ToString() => $"Leave[{Id}]";
    }
}