// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model.Attribute;
using Xunit;

namespace Vlingo.Cluster.Tests.Model.Attribute
{
    public class AttributeSetTest
    {
        [Fact]
        public void TestNamed()
        {
            var name = "test";
            var set1 = AttributeSet.Named(name);
            
            Assert.Equal(name, set1.Name);
            Assert.Equal(set1, AttributeSet.Named(name));
            Assert.NotEqual(set1, AttributeSet.Named("test-again"));
        }

        [Fact]
        public void TestAddIfAbsent()
        {
            var name = "test";
            var set1 = AttributeSet.Named(name);
            var attribute1 = Attribute<int>.From("attr1", 1);
            var tracked1 = set1.AddIfAbsent(attribute1);
            
            Assert.False(tracked1.IsAbsent);
            Assert.True(tracked1.IsPresent);
            
            var tracked2 = set1.AddIfAbsent(attribute1);
            
            Assert.Equal(tracked1, tracked2);
            Assert.True(Equals(tracked1, tracked2));
            
            var attribute2 = Attribute<string>.From("attr2", "One");
            var tracked3 = set1.AddIfAbsent(attribute2);
            
            Assert.NotEqual(tracked1, tracked3);
            Assert.NotEqual(tracked2, tracked3);
            Assert.Equal(tracked3, tracked3);
        }

        [Fact]
        public void TestAttributeNamed()
        {
            var name = "test";
            var attrName = "attr1";
            var set1 = AttributeSet.Named(name);
            var attribute1 = Attribute<int>.From(attrName, 1);
            var tracked1 = set1.AddIfAbsent(attribute1);
            
            set1.AddIfAbsent(Attribute<int>.From(attrName + "-a", 2));
            set1.AddIfAbsent(Attribute<int>.From(attrName + "-b", 3));
            set1.AddIfAbsent(Attribute<int>.From(attrName + "-c", 4));
            
            var tracked2 = set1.AttributeNamed(attrName);
            
            Assert.Equal(tracked1, tracked2);
            Assert.True(Equals(tracked1, tracked2));
            
            var tracked3 = set1.AttributeNamed(attrName + "+1");
            
            Assert.NotEqual(tracked1, tracked3);
            Assert.True(tracked3.IsAbsent);
            Assert.False(tracked3.IsPresent);
        }

        [Fact]
        public void TestRemove()
        {
            var name = "test";
            var attrName = "attr1";
            var set1 = AttributeSet.Named(name);
            var attribute1 = Attribute<int>.From(attrName, 1);
            var tracked1 = set1.AddIfAbsent(attribute1);
            
            set1.AddIfAbsent(Attribute<int>.From(attrName + "-a", 2));
            set1.AddIfAbsent(Attribute<int>.From(attrName + "-b", 3));
            set1.AddIfAbsent(Attribute<int>.From(attrName + "-c", 4));
            
            var tracked2 = set1.Remove(tracked1.Attribute);
            
            Assert.Equal(tracked1, tracked2);
            Assert.True(Equals(tracked1, tracked2));
            
            Assert.True(set1.AttributeNamed(attrName + "-a").IsPresent);
            Assert.True(set1.AttributeNamed(attrName + "-b").IsPresent);
            Assert.True(set1.AttributeNamed(attrName + "-c").IsPresent);
        }

        [Fact]
        public void TestReplace()
        {
            var name = "test";
            var attrName = "attr1";
            var set1 = AttributeSet.Named(name);
            var attribute1 = Attribute<int>.From(attrName, 1);
            var tracked1 = set1.AddIfAbsent(attribute1);
            
            set1.AddIfAbsent(Attribute<int>.From(attrName + "-a", 2));
            set1.AddIfAbsent(Attribute<int>.From(attrName + "-b", 3));
            set1.AddIfAbsent(Attribute<int>.From(attrName + "-c", 4));
            
            var tracked2 = set1.Replace(Attribute<int>.From(attrName, 2));
            
            Assert.NotEqual(tracked1, tracked2);

            Assert.Equal(tracked2, set1.AttributeNamed(attrName));

            Assert.True(set1.AttributeNamed(attrName).IsPresent);
            Assert.True(set1.AttributeNamed(attrName + "-a").IsPresent);
            Assert.True(set1.AttributeNamed(attrName + "-b").IsPresent);
            Assert.True(set1.AttributeNamed(attrName + "-c").IsPresent);
        }
    }
}