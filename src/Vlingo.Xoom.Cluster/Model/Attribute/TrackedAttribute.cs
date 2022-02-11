// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Xoom.Cluster.Model.Attribute;

public sealed class TrackedAttribute
{
    public static readonly TrackedAttribute Absent = new TrackedAttribute(null, null);
        
    public static TrackedAttribute Of<T>(AttributeSet set, Attribute<T> attribute)
    {
        var tid = TrackedIdFor(set, attribute);
        return new TrackedAttribute(tid, attribute);
    }
        
    public Attribute? Attribute { get; }
        
    public bool Distributed { get; }
        
    public string? Id { get; }

    public bool IsAbsent => Attribute == null;
        
    public bool IsDistributed => Distributed;
        
    public bool IsPresent => !IsAbsent;

    public Attribute ReplacingValueWith<T>(Attribute<T> other) => ((Attribute<T>)Attribute!).ReplacingValueWith(other);

    public bool SameAs(Attribute other) => Attribute != null && Attribute.Equals(other);
        
    public TrackedAttribute WithAttribute(Attribute attribute) => new TrackedAttribute(Id, attribute, false);
        
    public TrackedAttribute AsDistributed() => new TrackedAttribute(Id, Attribute, true);
        
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(TrackedAttribute))
        {
            return false;
        }

        var otherAttribute = (TrackedAttribute) obj;

        if (Attribute == null && Id == null)
        {
            return Distributed == otherAttribute.Distributed;
        }
            
        if (Attribute == null && Id != null)
        {
            return Distributed == otherAttribute.Distributed &&
                   Id.Equals(otherAttribute.Id);
        }

        if (Id == null && Attribute != null)
        {
            return Attribute.Equals(otherAttribute.Attribute) &&
                   Distributed == otherAttribute.Distributed;
        }
            
        return Attribute!.Equals(otherAttribute.Attribute) && 
               Distributed == otherAttribute.Distributed &&
               Id!.Equals(otherAttribute.Id);
    }

    public override int GetHashCode()
    {
        if (Attribute == null && Id == null)
        {
            return 31 * Distributed.GetHashCode();
        }

        if (Attribute == null)
        {
            return 31 * Distributed.GetHashCode() + Id!.GetHashCode();
        }

        if (Id == null)
        {
            return 31 * Attribute.GetHashCode() + Distributed.GetHashCode();
        }
            
        return 31 * Attribute.GetHashCode() + Distributed.GetHashCode() + Id.GetHashCode();
    }

    public override string ToString() => $"TrackedAttribute[attribute={Attribute}, distributed={Distributed}, id={Id}]";

    private static string TrackedIdFor<T>(AttributeSet set, Attribute<T> attribute) =>
        $"{set.Name}:{attribute.Name}";

    private TrackedAttribute(string? id, Attribute? attribute)
    {
        Attribute = attribute;
        Distributed = false;
        Id = attribute == null ? null : id;
    }

    private TrackedAttribute(string? id, Attribute? attribute, bool distributed)
    {
        Attribute = attribute;
        Distributed = distributed;
        Id = id;
    }
}