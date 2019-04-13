// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Cluster.Model.Attribute
{
    public sealed class TrackedAttribute<T>
    {
        public static readonly TrackedAttribute<T> Absent = new TrackedAttribute<T>(null, null);
        
        public static TrackedAttribute<T> Of(AttributeSet<T> set, Attribute<T> attribute)
        {
            var tid = TrackedIdFor(set, attribute);
            return new TrackedAttribute<T>(tid, attribute);
        }
        
        public Attribute<T> Attribute { get; }
        
        public bool Distributed { get; }
        
        public string Id { get; }

        public bool IsAbsent => Attribute == null;
        
        public bool IsDistributed => Distributed;
        
        public bool IsPresent => !IsAbsent;

        public Attribute<T> ReplacingValueWith(Attribute<T> other) => Attribute.ReplacingValueWith(other);

        public bool SameAs(Attribute<T> other) => Attribute.Equals(other);
        
        public TrackedAttribute<T> WithAttribute(Attribute<T> attribute) => new TrackedAttribute<T>(Id, attribute, false);
        
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(TrackedAttribute<T>))
            {
                return false;
            }

            var otherAttribute = (TrackedAttribute<T>) obj;
            return Attribute.Equals(otherAttribute.Attribute) && 
                   Distributed == otherAttribute.Distributed &&
                   Id.Equals(otherAttribute.Id);
        }

        public override int GetHashCode() => 31 * Attribute.GetHashCode() + Distributed.GetHashCode() + Id.GetHashCode();

        public override string ToString() => $"TrackedAttribute[attribute={Attribute}, distributed={Distributed}, id={Id}]";

        private static string TrackedIdFor(AttributeSet<T> set, Attribute<T> attribute) =>
            $"{set.Name}:{attribute.Name}";

        private TrackedAttribute(string id, Attribute<T> attribute)
        {
            Attribute = attribute;
            Distributed = false;
            Id = attribute == null ? null : id;
        }

        private TrackedAttribute(string id, Attribute<T> attribute, bool distributed)
        {
            Attribute = attribute;
            Distributed = distributed;
            Id = id;
        }
    }
}