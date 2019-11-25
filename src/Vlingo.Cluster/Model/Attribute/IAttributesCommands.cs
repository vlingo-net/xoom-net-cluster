// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

namespace Vlingo.Cluster.Model.Attribute
{
    public interface IAttributesCommands
    {
        void Add<T>(string attributeSetName, string attributeName, T value);
        
        void Replace<T>(string attributeSetName, string attributeName, T value);
        
        void Remove(string attributeSetName, string attributeName);
        
        void RemoveAll(string attributeSetName);
    }
}