// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Message;

public sealed class Leader : OperationalMessage
{
    public static Leader From(string content) => new Leader(OperationalMessagePartsBuilder.IdFrom(content));
        
    public Leader(Id id) : base(id)
    {
    }

    public override bool IsLeader => true;
        
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(Leader))
        {
            return false;
        }

        return Id.Equals(((Leader) obj).Id);
    }

    public override int GetHashCode() => 31 * Id.GetHashCode();

    public override string ToString() => $"Leader[{Id}]";
}