// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Vlingo.Cluster.Model.Attribute
{
    public sealed class AttributeSet<T>
    {
        private readonly ConcurrentDictionary<string, TrackedAttribute<T>> _attributes;
        
        public static AttributeSet<T> None => Named("__none");
        
        public static AttributeSet<T> Named(string name) => new AttributeSet<T>(name);

        public string Name { get; }

        public IEnumerable<TrackedAttribute<T>> All => _attributes.Values;

        public bool IsDefined => !IsNone;

        public bool IsNone => Equals(this, None);

        public TrackedAttribute<T> AddIfAbsent(Attribute<T> attribute)
        {
            var maybeAttribute = Find(attribute);

            if (maybeAttribute.IsAbsent)
            {
                var nowPresent = TrackedAttribute<T>.Of(this, attribute);
                return _attributes.AddOrUpdate(nowPresent.Id, nowPresent, (id, trackedAttribute) => nowPresent);
            }

            return maybeAttribute;
        }

        public TrackedAttribute<T> AttributeNamed(string name) => Find(name);

        public AttributeSet<T> Copy(AttributeSet<T> source)
        {
            var target = Named(source.Name);

            foreach (var attribute in _attributes.Values)
            {
                target._attributes.AddOrUpdate(attribute.Id, attribute, (id, trackedAttribute) => attribute);
            }

            return target;
        }

        public TrackedAttribute<T> Remove(Attribute<T> attribute)
        {
            var maybeAttribute = Find(attribute);

            if (maybeAttribute.IsPresent)
            {
                _attributes.TryRemove(maybeAttribute.Id, out maybeAttribute);
                return maybeAttribute;
            }

            return maybeAttribute;
        }

        public TrackedAttribute<T> Replace(Attribute<T> attribute)
        {
            var maybeAttribute = Find(attribute);
            
            if (maybeAttribute.IsPresent)
            {
                return _attributes.AddOrUpdate(
                    maybeAttribute.Id,
                    maybeAttribute, 
                   (id, trackedAttribute) => maybeAttribute.WithAttribute(attribute));
            }

            return maybeAttribute;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(AttributeSet<T>))
            {
                return false;
            }

            var otherAttribute = (AttributeSet<T>) obj;

            if (_attributes.Count != otherAttribute._attributes.Count)
            {
                return false;
            }

            foreach (var attributePair in _attributes)
            {
                if (!otherAttribute._attributes.ContainsKey(attributePair.Key))
                {
                    return false;
                }
                
                if (!_attributes[attributePair.Key].Equals(otherAttribute._attributes[attributePair.Key]))
                {
                    return false;
                }
            }
            
            return Name.Equals(otherAttribute.Name);
        }

        public override int GetHashCode()
        {
            var hashCode = 31 * Name.GetHashCode();

            foreach (var attribute in _attributes.Values)
            {
                hashCode += attribute.GetHashCode();
            }

            return hashCode;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var trackedAttribute in _attributes)
            {
                builder.AppendLine($"({trackedAttribute.Key}, {trackedAttribute.Value})");
            }
            
            return $"AttributeSet[name={Name}, attributes=[{builder}]]";
        }

        private AttributeSet(string name)
        {
            Name = name;
            _attributes = new ConcurrentDictionary<string, TrackedAttribute<T>>(16, 128);
        }

        private TrackedAttribute<T> Find(Attribute<T> attribute) => Find(attribute.Name);

        private TrackedAttribute<T> Find(string name)
        {
            foreach (var id in _attributes.Values)
            {
                if (id.Attribute.Name.Equals(name))
                {
                    return id;
                }
            }

            return TrackedAttribute<T>.Absent;
        }
    }
}