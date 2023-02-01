// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Xoom.Cluster.Model.Attribute;

public abstract class Attribute
{
    public string? Name { get; protected set; }
        
    public AttributeType Type { get; protected set; }

    public abstract string? ToStringValue();
}
    
public sealed class Attribute<T> : Attribute
{
    public static Attribute<T> Undefined => From("__undefined", AttributeType.String, default);

    public static Attribute<T> From(string? name, T value) => new Attribute<T>(name, value, TypeOf(typeof(T)));

    public static Attribute<T> From(string? name, AttributeType attributeType, string? value)
    {
        var typedValue = TypeValue(attributeType, value);
        return new Attribute<T>(name, (T)typedValue!, attributeType);
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

    public Attribute(string? name, T value, AttributeType attributeType)
    {
        Name = name;
        Value = value;
        Type = attributeType;
    }
        
    public T Value { get; }

    public bool IsUndefined => Equals(Undefined);

    public override string? ToStringValue() => Value?.ToString();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(Attribute<T>))
        {
            return false;
        }

        var otherAttribute = (Attribute<T>) obj;

        if (Value == null && Name == null)
        {
            return Type == otherAttribute.Type;
        }

        if (Value == null)
        {
            return Name!.Equals(otherAttribute.Name) && 
                   Type == otherAttribute.Type;
        }
            
        if (Name == null)
        {
            return Value.Equals(otherAttribute.Value) &&
                   Type == otherAttribute.Type;
        }
            
        return Name.Equals(otherAttribute.Name) && 
               Value.Equals(otherAttribute.Value) &&
               Type == otherAttribute.Type;
    }

    public override int GetHashCode()
    {
        if (Name == null && Value == null)
        {
            return 31 * Type.GetHashCode();
        }
            
        if (Name == null && Value != null)
        {
            return 31 * Value.GetHashCode() + Type.GetHashCode();
        }
            
        if (Value == null && Name != null)
        {
            return 31 * Name.GetHashCode() + Type.GetHashCode();
        }
            
        return 31 * Name!.GetHashCode() + Value!.GetHashCode() + Type.GetHashCode();
            
    }

    public override string ToString() => $"Attribute[name={Name}, value={Value}, type={Type}]";

    private static AttributeType TypeOf(Type? type)
    {
        switch (type?.FullName)
        {
            case "System.String":
                return AttributeType.String;
            case "System.Int32":
                return AttributeType.Integer;
            case "System.Int64":
                return AttributeType.Long;
            case "System.Boolean":
                return AttributeType.Boolean;
            case "System.Byte":
                return AttributeType.Byte;
            case "System.Double":
                return AttributeType.Double;
            case "System.Single":
                return AttributeType.Float;
            case "System.Int16":
                return AttributeType.Short;
            case "System.Char":
                return AttributeType.Character;
            case "System.Decimal":
                return AttributeType.Decimal;
            default:
                throw new ArgumentException($"The type '{type?.FullName}' is not recognized.");
        }
    }

    public Attribute<T> ReplacingValueWith(Attribute<T> other)
    {
        if (Type != other.Type)
        {
            throw new ArgumentException("Source and target attributes have different types.");
        }
            
        return new Attribute<T>(Name, other.Value, Type);
    }

    private static object? TypeValue(AttributeType attributeType, string? value)
    {
        switch (attributeType)
        {
            case AttributeType.String:
                return value;
            case AttributeType.Integer:
                return int.Parse(value!);
            case AttributeType.Long:
                return long.Parse(value!);
            case AttributeType.Boolean:
                return bool.Parse(value!);
            case AttributeType.Byte:
                return byte.Parse(value!);
            case AttributeType.Double:
                return double.Parse(value!);
            case AttributeType.Float:
                return float.Parse(value!);
            case AttributeType.Short:
                return short.Parse(value!);
            case AttributeType.Character:
                return char.Parse(value!);
            case AttributeType.Decimal: 
                return decimal.Parse(value!);
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
    String,
    Decimal
}