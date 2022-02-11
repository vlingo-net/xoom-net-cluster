// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Vlingo.Xoom.Cluster.Model.Attribute;

internal class AttributeSetRepository
{
    private readonly ConcurrentDictionary<string, AttributeSet> _all;

    internal AttributeSetRepository()
    {
        _all = new ConcurrentDictionary<string, AttributeSet>(16, 16);
    }

    internal void Add(AttributeSet set) => _all.AddOrUpdate(set.Name!, set, (s, attributeSet) => set);

    internal AttributeSet AttributeSetOf(string name)
    {
        if (_all.TryGetValue(name, out var attributeSet))
        {
            return attributeSet;
        }

        return AttributeSet.None;
    }

    internal void Remove(string name) => _all.TryRemove(name, out _);

    internal void RemoveAll() => _all.Clear();
        
    internal void SyncWith(AttributeSet set) => _all.AddOrUpdate(set.Name!, set, (s, attributeSet) => set);

    internal IEnumerable<AttributeSet> All => _all.Values;
}