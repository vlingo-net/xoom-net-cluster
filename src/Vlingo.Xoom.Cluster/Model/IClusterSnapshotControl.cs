// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq.Expressions;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model.Application;
using Vlingo.Xoom.Common.Expressions;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model
{
    public interface IClusterSnapshotControl
    {
        void ShutDown();
    }

    public class ClusterSnapshotControlFactory
    {
        public static (IClusterSnapshotControl, ILogger) Instance<TActor>(
            World world,
            Expression<Func<Node, TActor>> instantiator,
            string nodeName)
        {
            var clusterApplicationActor = Properties.Instance.ClusterApplicationStageName();
            var applicationStage = world.StageNamed(clusterApplicationActor);
            return Instance(world, applicationStage, instantiator, nodeName);
        }
        
        public static (IClusterSnapshotControl, ILogger) Instance<TActor>(
            World world,
            Stage stage,
            Expression<Func<Node, TActor>> instantiator,
            string nodeName)
        {
            var initializer = new ClusterSnapshotInitializer(nodeName, Properties.Instance, world.DefaultLogger);
            var node = initializer.LocalNode;

            var curriedExpression = instantiator.Curry(node);

            var application = ClusterApplicationFactory.Instance(stage, curriedExpression);

            var control =
                world.ActorFor<IClusterSnapshotControl>(() => new ClusterSnapshotActor(initializer, application), $"cluster-snapshot-{nodeName}");
            
            return (control, world.DefaultLogger);
        }
    }
}