// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model.Message;
using Vlingo.Xoom.Cluster.Model.Outbound;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Nodes;

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
        var localLiveNode = stage.ActorFor<ILocalLiveNode>(
            () => new LocalLiveNodeActor(node, snapshot, registry, outbound, configuration), "local-live-node");

        return localLiveNode;
    }
}