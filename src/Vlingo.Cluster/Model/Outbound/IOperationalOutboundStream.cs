// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Message;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;

namespace Vlingo.Cluster.Model.Outbound
{
    using Vlingo.Wire.Node;
    
    public interface IOperationalOutboundStream : IStoppable
    {
        void Close(Id id);
        
        Task Application(ApplicationSays says, IEnumerable<Node> unconfirmedNodes);
        
        Task Directory(IEnumerable<Node> allLiveNodes);
        
        Task Elect(IEnumerable<Node> allGreaterNodes);
        
        Task Join();
        
        Task Leader();
        
        Task Leader(Id id);
        
        Task Leave();
        
        void Open(Id id);
        
        Task Ping(Id targetNodeId);
        
        Task Pulse(Id targetNodeId);
        
        Task Pulse();
        
        Task Split(Id targetNodeId, Id currentLeaderId);
        
        Task Vote(Id targetNodeId);
    }

    public static class OperationalOutboundStreamFactory
    {
        public static IOperationalOutboundStream Instance(
            Stage stage,
            Node node,
            IManagedOutboundChannelProvider provider,
            ByteBufferPool byteBufferPool)
        {
            var definition =
                    Definition.Has<OperationalOutboundStreamActor>(
                        Definition.Parameters(node, provider, byteBufferPool),
            "cluster-operational-outbound-stream");
            
            var operationalOutboundStream =
                stage.ActorFor<IOperationalOutboundStream>(definition);
    
            return operationalOutboundStream;
        }
    }
}