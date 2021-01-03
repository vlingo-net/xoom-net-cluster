// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Actors;
using Vlingo.Cluster.Model.Application;

namespace Vlingo.Cluster.Model
{
    public interface IClusterSnapshotControl
    {
        void ShutDown();
    }

    public class ClusterSnapshotControlFactory
    {
        public static (IClusterSnapshotControl, ILogger) Instance(World world, string nodeName)
        {
            var initializer = new ClusterSnapshotInitializer(nodeName, Properties.Instance, world.DefaultLogger);
            
            var application = ClusterApplicationFactory.Instance(world, initializer.LocalNode);

            var control =
                world.ActorFor<IClusterSnapshotControl>(() => new ClusterSnapshotActor(initializer, application), $"cluster-snapshot-{nodeName}");
            
            return (control, world.DefaultLogger);
        }
    }
}