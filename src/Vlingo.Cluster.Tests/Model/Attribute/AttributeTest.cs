// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model.Attribute;

using Xunit;

namespace Vlingo.Cluster.Tests.Model.Attribute
{   
    public class AttributeTest
    {
        [Fact]
        public void TestByteValue()
        {
            var attribute1 = Attribute<byte>.From("byte1", 1);
            
            Assert.Equal(1, attribute1.Value);
            Assert.Equal((byte)1, attribute1.Value);
            Assert.Equal(AttributeType.Byte, attribute1.Type);
            
            var attribute2 = Attribute<byte>.From("byte2", 2);
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<byte>.From("byte2", 2));
        }
        
        [Fact]
        public void TestShortValue()
        {
            var attribute1 = Attribute<short>.From("short1", 1);
            
            Assert.Equal(1, attribute1.Value);
            Assert.Equal(AttributeType.Short, attribute1.Type);
            
            var attribute2 = Attribute<short>.From("short2", 2);
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<short>.From("short2", 2));
        }
        
        [Fact]
        public void TestIntegerValue()
        {
            var attribute1 = Attribute<int>.From("int1", 1);
            
            Assert.Equal(1, attribute1.Value);
            Assert.Equal(AttributeType.Integer, attribute1.Type);
            
            var attribute2 = Attribute<int>.From("int2", 2);
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<int>.From("int2", 2));
        }
        
        [Fact]
        public void TestLongValue()
        {
            var attribute1 = Attribute<long>.From("long1", 1);
            
            Assert.Equal(1, attribute1.Value);
            Assert.Equal(AttributeType.Long, attribute1.Type);
            
            var attribute2 = Attribute<long>.From("long2", 2);
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<long>.From("long2", 2));
        }
        
        [Fact]
        public void TestCharacterValue()
        {
            var attribute1 = Attribute<char>.From("char1", 'A');
            
            Assert.Equal('A', attribute1.Value);
            Assert.Equal(AttributeType.Character, attribute1.Type);
            
            var attribute2 = Attribute<char>.From("char2", 'B');
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<char>.From("char2", 'B'));
        }
        
        [Fact]
        public void TestFloatValue()
        {
            var attribute1 = Attribute<float>.From("float1", 1.1f);
            
            Assert.Equal(1.1f, attribute1.Value);
            Assert.Equal(AttributeType.Float, attribute1.Type);
            
            var attribute2 = Attribute<float>.From("float2", 2.2f);
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<float>.From("float2", 2.2f));
        }
        
        [Fact]
        public void TestDoubleValue()
        {
            var attribute1 = Attribute<double>.From("double1", 1.1);
            
            Assert.Equal(1.1, attribute1.Value);
            Assert.Equal(AttributeType.Double, attribute1.Type);
            
            var attribute2 = Attribute<double>.From("double2", 2.2);
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<double>.From("double2", 2.2));
        }
        
        [Fact]
        public void TestDecimalValue()
        {
            var attribute1 = Attribute<decimal>.From("double1", 1.1M);
            
            Assert.Equal(1.1M, attribute1.Value);
            Assert.Equal(AttributeType.Decimal, attribute1.Type);
            
            var attribute2 = Attribute<decimal>.From("decimal2", 2.2M);
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<decimal>.From("decimal2", 2.2M));
        }
        
        [Fact]
        public void TestBooleanValue()
        {
            var attribute1 = Attribute<bool>.From("bool1", true);
            
            Assert.Equal(true, attribute1.Value);
            Assert.Equal(AttributeType.Boolean, attribute1.Type);
            
            var attribute2 = Attribute<bool>.From("bool2", false);
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<bool>.From("bool2", false));
        }
        
        [Fact]
        public void TestStringValue()
        {
            var attribute1 = Attribute<string>.From("string1", "A");
            
            Assert.Equal("A", attribute1.Value);
            Assert.Equal(new string("A"), attribute1.Value);
            Assert.Equal(AttributeType.String, attribute1.Type);
            
            var attribute2 = Attribute<string>.From("string2", new string("B"));
            
            Assert.NotEqual(attribute1, attribute2);
            Assert.Equal(attribute2, Attribute<string>.From("string2", new string("B")));
        }

        [Fact]
        public void TestFromAttributeType()
        {
            var attribute1 = Attribute<int>.From("int1", AttributeType.Integer, "101");
            Assert.Equal(101, attribute1.Value);
        }
    }
}