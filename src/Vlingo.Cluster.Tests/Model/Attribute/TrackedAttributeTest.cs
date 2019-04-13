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
    public class TrackedAttributeTest
    {
        private readonly AttributeSet<int> _set = AttributeSet<int>.Named("test");

        [Fact]
        public void TestTrackedAttributeOf()
        {
            var attr1 = Attribute<int>.From("attr1", 1);
            var tracked1 = TrackedAttribute<int>.Of(_set, attr1);
            
            Assert.Equal(attr1, tracked1.Attribute);
            
            var attr2 = Attribute<int>.From("attr1", 1);
            var tracked2 = TrackedAttribute<int>.Of(_set, attr2);
            
            Assert.Equal(attr1, attr2);
            Assert.Equal(attr2, tracked2.Attribute);
            Assert.Equal(tracked1, tracked2);
        }
    }
}