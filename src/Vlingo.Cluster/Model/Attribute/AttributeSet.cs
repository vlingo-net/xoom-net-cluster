// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Concurrent;

namespace Vlingo.Cluster.Model.Attribute
{
    public sealed class AttributeSet
    {
        private readonly ConcurrentDictionary<string, TrackedAttribute> _attributes;
        
        public static AttributeSet None => Named("__none");
        
        public static AttributeSet Named(string name) => new AttributeSet(name);

        public string Name { get; }

        private AttributeSet(string name)
        {
            Name = name;
            _attributes = new ConcurrentDictionary<string, TrackedAttribute>(16, 128);
        }
    }
}