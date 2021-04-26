// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Cluster.Model.Message
{
    public sealed class Vote : OperationalMessage
    {
        public static Vote From(string content) =>  new Vote(OperationalMessagePartsBuilder.IdFrom(content));
        
        public Vote(Id id) : base(id)
        {
        }

        public override bool IsVote => true;
        
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Vote))
            {
                return false;
            }

            return Id.Equals(((Vote) obj).Id);
        }

        public override int GetHashCode() => 31 * Id.GetHashCode();

        public override string ToString() => $"Vote[{Id}]";
    }
}