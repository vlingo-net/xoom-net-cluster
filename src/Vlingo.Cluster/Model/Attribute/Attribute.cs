// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Cluster.Model.Attribute
{
    public sealed class Attribute<T>
    {
        public static Attribute<T> Undefined => From("__undefined", AttributeType.String, default);

        public static Attribute<T> From(string name, T value) => new Attribute<T>(name, value, TypeOf(typeof(T)));

        public static Attribute<T> From(string name, AttributeType attributeType, T value)
        {
            var typedValue = TypeValue(attributeType, value);
            return new Attribute<T>(name, typedValue, attributeType);
        }

        public static AttributeType TypeOfAttribute(string fullName)
        {
            try
            {
                var type = System.Type.GetType(fullName);
                return TypeOf(type);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"The type '{fullName}' is not recognized.", e);
            }
        }

        public Attribute(string name, T value, AttributeType attributeType)
        {
            Name = name;
            Value = value;
            Type = attributeType;
        }
        
        public string Name { get; }
        
        public AttributeType Type { get; }
        
        public T Value { get; }

        public bool IsUndefined => Equals(Undefined);

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(Attribute<T>))
            {
                return false;
            }

            var otherAttribute = (Attribute<T>) obj;
            return Name.Equals(otherAttribute.Name) && 
                   Value.Equals(otherAttribute.Value) &&
                   Type == otherAttribute.Type;
        }

        public override int GetHashCode() => 31 * Name.GetHashCode() + Value.GetHashCode() + Type.GetHashCode();

        public override string ToString() => $"Attribute[name={Name}, value={Value}, type={Type}]";

        private static AttributeType TypeOf(Type type)
        {
            switch (type.FullName)
            {
                case "System.String":
                    return AttributeType.String;
                default:
                    throw new ArgumentException($"The type '{type.FullName}' is not recognized.");
            }
        }

        public Attribute<T> ReplacingValueWith(Attribute<T> other)
        {
            if (Type != other.Type)
            {
                throw new ArgumentException("Source and target attributes have different types.");
            }
            
            return new Attribute<T>(Name, Value, Type);
        }

        private static T TypeValue(AttributeType attributeType, T value)
        {
            switch (attributeType)
            {
                case AttributeType.String:
                    return value;
                default:
                    throw new ArgumentException();
            }
        }
    }
    
    // Named differently from Java because of .NET name clashes.
    public enum AttributeType
    {
        Byte,
        Short,
        Integer,
        Long,
        Character,
        Float,
        Double,
        Boolean,
        String
    }
}