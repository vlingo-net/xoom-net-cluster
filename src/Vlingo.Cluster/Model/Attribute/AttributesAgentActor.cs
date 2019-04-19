// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Actors;
using Vlingo.Common;
using Vlingo.Wire.Message;

namespace Vlingo.Cluster.Model.Attribute
{
    using Vlingo.Wire.Node;
    
    public class AttributesAgentActor : Actor, IAttributesAgent
    {
        private readonly AttributesClient _client;
        private readonly IConfiguration _configuration;
        private readonly IConfirmationInterest _confirmationInterest;
        private readonly ConfirmingDistributor _confirmingDistributor;
        private readonly Node _node;
        private readonly RemoteAttributeRequestHandler _remoteRequestHandler;
        private readonly AttributeSetRepository _repository;
        
        public void Add<T>(string attributeSetName, string attributeName, T value)
        {
            throw new System.NotImplementedException();
        }

        public void Replace<T>(string attributeSetName, string attributeName, T value)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string attributeSetName, string attributeName)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAll(string attributeSetName)
        {
            throw new System.NotImplementedException();
        }

        public void Synchronize(Wire.Node.Node node)
        {
            throw new System.NotImplementedException();
        }

        public void HandleInboundStreamMessage(AddressType addressType, RawMessage message)
        {
            throw new System.NotImplementedException();
        }

        public void IntervalSignal(IScheduled<object> scheduled, object data)
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public bool IsStopped { get; }
    }
}