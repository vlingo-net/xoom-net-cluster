// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;

namespace Vlingo.Xoom.Cluster.Model.Attribute;

public class AttributesClient : IAttributesProtocol
{
    private readonly IAttributesAgent _agent;
    private readonly AttributeSetRepository _repository;
    private static volatile object _syncRoot = new object();

    public static AttributesClient With(IAttributesAgent agent)
    {
        lock (_syncRoot)
        {
            return new AttributesClient(agent, new AttributeSetRepository());
        }
    }

    public void Add<T>(string attributeSetName, string attributeName, T value) => _agent.Add(attributeSetName, attributeName, value);

    public void Replace<T>(string attributeSetName, string attributeName, T value) => _agent.Replace(attributeSetName, attributeName, value);

    public void Remove(string attributeSetName, string attributeName) => _agent.Remove(attributeSetName, attributeName);

    public void RemoveAll(string attributeSetName) => _agent.RemoveAll(attributeSetName);

    public IEnumerable<Attribute> AllOf(string attributeSetName)
    {
        var all = new List<Attribute>();
        var set = _repository.AttributeSetOf(attributeSetName);
        if (set.IsDefined)
        {
            foreach (var tracked in set.All)
            {
                if (tracked.IsPresent)
                {
                    all.Add(tracked.Attribute!);
                }
            }
        }
            
        return all;
    }

    public Attribute<T> Attribute<T>(string attributeSetName, string? attributeName)
    {
        var set = _repository.AttributeSetOf(attributeSetName);
        if (set.IsDefined)
        {
            var tracked = set.AttributeNamed(attributeName);
            if (tracked.IsPresent)
            {
                return (Attribute<T>) tracked.Attribute!;
            }
        }
            
        return Model.Attribute.Attribute<T>.Undefined;
    }

    public override string ToString() => $"AttributesClient[agent={_agent} repository={_repository}]";
        
    internal void SyncWith(AttributeSet set)
    {
        _repository.SyncWith(set.Copy(set));
    }

    internal void SyncWithout(AttributeSet set)
    {
        _repository.Remove(set.Name!);
    }

    public IEnumerable<AttributeSet> All => _repository.All;
        
    private AttributesClient(IAttributesAgent agent, AttributeSetRepository repository)
    {
        _agent = agent;
        _repository = repository;
    }
}