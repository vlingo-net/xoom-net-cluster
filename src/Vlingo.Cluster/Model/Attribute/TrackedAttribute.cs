// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Cluster.Model.Attribute
{
    public sealed class TrackedAttribute
    {
        public static readonly TrackedAttribute Absent = new TrackedAttribute(null, null);
        
        public Attribute<object> Attribute { get; }
        
        public bool Distributed { get; }
        
        public string Id { get; }

        public bool IsAbsent => Attribute == null;
        
        public bool IsDistributed => Distributed;
        
        public bool IsPresent => !IsAbsent;

        public Attribute<object> ReplacingValueWith(Attribute<object> other) => Attribute.ReplacingValueWith(other);

        public bool SameAs(Attribute<object> other) => Attribute.Equals(other);
        
        public TrackedAttribute WithAttribute(Attribute<object> attribute) => new TrackedAttribute(Id, attribute, false);

        static TrackedAttribute Of(AttributeSet set, Attribute<object> attribute)
        {
            var tid = TrackedIdFor(set, attribute);
            return new TrackedAttribute(tid, attribute);
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(TrackedAttribute))
            {
                return false;
            }

            var otherAttribute = (TrackedAttribute) obj;
            return Attribute.Equals(otherAttribute.Attribute) && 
                   Distributed == otherAttribute.Distributed &&
                   Id.Equals(otherAttribute.Id);
        }

        public override int GetHashCode() => 31 * Attribute.GetHashCode() + Distributed.GetHashCode() + Id.GetHashCode();

        public override string ToString() => $"TrackedAttribute[attribute={Attribute}, distributed={Distributed}, id={Id}]";

        private static string TrackedIdFor(AttributeSet set, Attribute<object> attribute) =>
            $"{set.Name}:{attribute.Name}";

        private TrackedAttribute(string id, Attribute<object> attribute)
        {
            Attribute = attribute;
            Distributed = false;
            Id = attribute == null ? null : id;
        }

        private TrackedAttribute(string id, Attribute<object> attribute, bool distributed)
        {
            Attribute = attribute;
            Distributed = distributed;
            Id = id;
        }
    }
}