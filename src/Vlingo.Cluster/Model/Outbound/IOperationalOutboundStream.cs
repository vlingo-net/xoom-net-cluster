// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Cluster.Model.Message;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Wire.Fdx.Outbound;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Cluster.Model.Outbound
{
    public interface IOperationalOutboundStream : IStoppable
    {
        void Close(Id id);
        
        void Application(ApplicationSays says, IEnumerable<Node> unconfirmedNodes);
        
        void Directory(IEnumerable<Node> allLiveNodes);
        
        void Elect(IEnumerable<Node> allGreaterNodes);
        
        void Join();
        
        void Leader();
        
        void Leader(Id id);
        
        void Leave();
        
        void Open(Id id);
        
        void Ping(Id targetNodeId);
        
        void Pulse(Id targetNodeId);
        
        void Pulse();
        
        void Split(Id targetNodeId, Id currentLeaderId);
        
        void Vote(Id targetNodeId);
    }

    public static class OperationalOutboundStreamFactory
    {
        public static IOperationalOutboundStream Instance(
            Stage stage,
            Node node,
            IManagedOutboundChannelProvider provider,
            ConsumerByteBufferPool byteBufferPool)
        {
            var operationalOutboundStream =
                stage.ActorFor<IOperationalOutboundStream>(() => new OperationalOutboundStreamActor(node, provider, byteBufferPool), "cluster-operational-outbound-stream");
    
            return operationalOutboundStream;
        }
    }
}