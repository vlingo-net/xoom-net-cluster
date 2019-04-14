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
    public sealed class AttributeSet
    {
        private readonly ConcurrentDictionary<string, TrackedAttribute> _attributes;
        
        public static AttributeSet None => Named("__none");
        
        public static AttributeSet Named(string name) => new AttributeSet(name);

        public string Name { get; }

        public IEnumerable<TrackedAttribute> All => _attributes.Values;

        public bool IsDefined => !IsNone;

        public bool IsNone => Equals(this, None);

        public TrackedAttribute AddIfAbsent<T>(Attribute<T> attribute)
        {
            var maybeAttribute = Find(attribute);

            if (maybeAttribute.IsAbsent)
            {
                var nowPresent = TrackedAttribute.Of(this, attribute);
                return _attributes.AddOrUpdate(
                    nowPresent.Id,
                    nowPresent, 
                    (id, trackedAttribute) => nowPresent);
            }

            return maybeAttribute;
        }

        public TrackedAttribute AttributeNamed<T>(string name) => Find<T>(name);

        public AttributeSet Copy(AttributeSet source)
        {
            var target = Named(source.Name);

            foreach (var attribute in _attributes.Values)
            {
                target._attributes.AddOrUpdate(attribute.Id, attribute, (id, trackedAttribute) => attribute);
            }

            return target;
        }

        public TrackedAttribute Remove<T>(Attribute<T> attribute)
        {
            var maybeAttribute = Find(attribute);

            if (maybeAttribute.IsPresent)
            {
                _attributes.TryRemove(maybeAttribute.Id, out var removedAttribute);
                return removedAttribute;
            }

            return maybeAttribute;
        }

        public TrackedAttribute Replace(Attribute<object> attribute)
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
            if (obj == null || obj.GetType() != typeof(AttributeSet))
            {
                return false;
            }

            var otherAttribute = (AttributeSet) obj;

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
            _attributes = new ConcurrentDictionary<string, TrackedAttribute>(16, 128);
        }

        private TrackedAttribute Find<T>(Attribute<T> attribute) => Find<T>(attribute.Name);

        private TrackedAttribute Find<T>(string name)
        {
            foreach (var id in _attributes.Values)
            {
                if (id.Attribute.Name.Equals(name))
                {
                    return id;
                }
            }

            return TrackedAttribute.Absent;
        }
    }
}