// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Actors;
using Vlingo.Cluster.Model.Message;
using Vlingo.Cluster.Model.Outbound;

namespace Vlingo.Cluster.Model.Node
{
    using Vlingo.Wire.Node;
    
    public interface ILocalLiveNode : IStoppable
    {
        void Handle(OperationalMessage message);
        void RegisterNodeSynchronizer(INodeSynchronizer nodeSynchronizer);
    }

    public static class LocalLiveNodeFactory
    {
        public static ILocalLiveNode Instance(
            Stage stage,
            Node node,
            IClusterSnapshot snapshot,
            IRegistry registry,
            IOperationalOutboundStream outbound,
            IConfiguration configuration)
        {
            var definition = Definition.Has<LocalLiveNodeActor>(
                Definition.Parameters(node, snapshot, registry, outbound, configuration),"local-live-node");
    
            var localLiveNode = stage.ActorFor<ILocalLiveNode>(definition);
    
            return localLiveNode;
        }
    }
}