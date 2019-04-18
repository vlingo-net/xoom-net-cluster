// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Message
{
    public sealed class Ping : OperationalMessage
    {
        public static Ping From(string content) =>  new Ping(OperationalMessagePartsBuilder.IdFrom(content));
        
        public Ping(Id id) : base(id)
        {
        }

        public override bool IsPing => true;
        
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Ping))
            {
                return false;
            }

            return Id.Equals(((Ping) obj).Id);
        }

        public override int GetHashCode() => 31 * Id.GetHashCode();

        public override string ToString() => $"Ping[{Id}]";
    }
}