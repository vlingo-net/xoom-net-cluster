// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Actors;
using Vlingo.Cluster.Model.Application;
using Vlingo.Cluster.Model.Outbound;
using Vlingo.Xoom.Common;
using Vlingo.Wire.Fdx.Inbound;

namespace Vlingo.Cluster.Model.Attribute
{
    using Vlingo.Wire.Node;
    
    public interface IAttributesAgent : IAttributesCommands, INodeSynchronizer, IInboundStreamInterest, IScheduled<object>, IStoppable
    {
    }
    
    public static class AttributesAgentFactory
    {
        public static IAttributesAgent Instance(
            Stage stage,
            Node node,
            IClusterApplication application,
            IOperationalOutboundStream outbound,
            IConfiguration configuration)
        {
            var attributesAgent = stage.ActorFor<IAttributesAgent>(
                () => new AttributesAgentActor(node, application, outbound, configuration), "attributes-agent");
            
            return attributesAgent;
        }
    }
}