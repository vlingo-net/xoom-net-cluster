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
    public class AttributeSetTest
    {
        [Fact]
        public void TestNamed()
        {
            var name = "test";
            var set1 = AttributeSet<int>.Named(name);
            
            Assert.Equal(name, set1.Name);
            Assert.Equal(set1, AttributeSet<int>.Named(name));
            Assert.NotEqual(set1, AttributeSet<int>.Named("test-again"));
        }
    }
}