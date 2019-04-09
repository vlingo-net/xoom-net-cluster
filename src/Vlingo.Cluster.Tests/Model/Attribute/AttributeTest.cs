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
    }
}