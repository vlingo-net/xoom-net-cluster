// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Cluster.Model.Attribute;
using Xunit;

namespace Vlingo.Xoom.Cluster.Tests.Model.Attribute
{
    public class TrackedAttributeTest
    {
        private readonly AttributeSet _set = AttributeSet.Named("test");

        [Fact]
        public void TestTrackedAttributeOf()
        {
            var attr1 = Attribute<int>.From("attr1", 1);
            var tracked1 = TrackedAttribute.Of(_set, attr1);
            
            Assert.Equal(attr1, tracked1.Attribute);
            
            var attr2 = Attribute<int>.From("attr1", 1);
            var tracked2 = TrackedAttribute.Of(_set, attr2);
            
            Assert.Equal(attr1, attr2);
            Assert.Equal(attr2, tracked2.Attribute);
            Assert.Equal(tracked1, tracked2);
        }

        [Fact]
        public void TestAsDistributed()
        {
            var attr1 = Attribute<int>.From("attr1", 1);
            var tracked1 = TrackedAttribute.Of(_set, attr1);
            
            Assert.False(tracked1.Distributed);

            var tracked2 = tracked1.AsDistributed();
            
            Assert.True(tracked2.Distributed);
            
            Assert.Equal(tracked1.Attribute, tracked2.Attribute);
            Assert.Equal(tracked1.Id, tracked2.Id);
        }

        [Fact]
        public void TestAbsentPresent()
        {
            var attr1 = Attribute<int>.From("attr1", 1);
            var tracked1 = TrackedAttribute.Of(_set, attr1);
            
            Assert.False(tracked1.IsAbsent);
            Assert.True(tracked1.IsPresent);
            
            Assert.True(TrackedAttribute.Absent.IsAbsent);
            Assert.False(TrackedAttribute.Absent.IsPresent);
        }

        [Fact]
        public void TestWithAttribute()
        {
            var attr1 = Attribute<int>.From("attr1", 1);
            var tracked1 = TrackedAttribute.Of(_set, attr1);
            var attr1ValueModified = Attribute<int>.From("attr1", 2);
            var tracked2 = tracked1.WithAttribute(attr1ValueModified);
            
            Assert.NotEqual(attr1, tracked2.Attribute);
            Assert.NotEqual(tracked1.Attribute, tracked2.Attribute);
            Assert.Equal(attr1ValueModified, tracked2.Attribute);
            Assert.Equal(tracked1.Distributed, tracked2.Distributed);
            Assert.Equal(tracked1.Id, tracked2.Id);
        }
    }
}