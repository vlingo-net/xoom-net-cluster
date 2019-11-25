// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Linq;
using Vlingo.Cluster.Model.Attribute;
using Xunit;

namespace Vlingo.Cluster.Tests.Model.Attribute
{
    public class AttributeSetRepositoryTest
    {
        private readonly AttributeSetRepository _repository = new AttributeSetRepository();
        private int _times = 0;

        [Fact]
        public void TestAdd()
        {
            var set1 = AttributeSetFixture("add");
            
            _repository.Add(set1);
    
            Assert.Equal(set1, _repository.AttributeSetOf("add"));
        }

        [Fact]
        public void TestAll()
        {
            var set1 = AttributeSetFixture("set1");
            var set2 = AttributeSetFixture("set2");
            var set3 = AttributeSetFixture("set3");
            
            _repository.Add(set1);
            _repository.Add(set2);
            _repository.Add(set3);
    
            Assert.Equal(3, _repository.All.Count());
        }

        [Fact]
        public void TestAttributeSetOf()
        {
            var set1 = AttributeSetFixture("set1");
            var set2 = AttributeSetFixture("set2");
            var set3 = AttributeSetFixture("set3");
            
            _repository.Add(set1);
            _repository.Add(set2);
            _repository.Add(set3);
            
            Assert.Equal(set3, _repository.AttributeSetOf("set3"));
            Assert.Equal(set3, _repository.AttributeSetOf("set3"));
            Assert.Equal(set3, _repository.AttributeSetOf("set3"));
        }

        [Fact]
        public void TestRemove()
        {
            var set1 = AttributeSetFixture("set1");
            var set2 = AttributeSetFixture("set2");
            var set3 = AttributeSetFixture("set3");
            
            _repository.Add(set1);
            _repository.Add(set2);
            _repository.Add(set3);
            
            _repository.Remove("set1");
            Assert.Equal(AttributeSet.None, _repository.AttributeSetOf("set1"));
            Assert.NotNull(_repository.AttributeSetOf("set2"));
            Assert.NotNull(_repository.AttributeSetOf("set3"));
            
            _repository.Remove("set2");
            Assert.Equal(AttributeSet.None, _repository.AttributeSetOf("set1"));
            Assert.Equal(AttributeSet.None, _repository.AttributeSetOf("set2"));
            Assert.NotNull(_repository.AttributeSetOf("set3"));
            
            _repository.Remove("set3");
            Assert.Equal(AttributeSet.None, _repository.AttributeSetOf("set1"));
            Assert.Equal(AttributeSet.None, _repository.AttributeSetOf("set2"));
            Assert.Equal(AttributeSet.None, _repository.AttributeSetOf("set3"));
        }
        
        private AttributeSet AttributeSetFixture(string name) {
            var set = AttributeSet.Named(name);
    
            _times = (_times * 2) + 1;
    
            for (var idx = 0; idx < _times; ++idx) {
                var current = _times + idx;
      
                set.AddIfAbsent(Attribute<int>.From($"attr{current}", current));
            }
    
            return set;
        }
    }
}