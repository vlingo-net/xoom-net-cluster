// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Xoom.Actors;

namespace Vlingo.Xoom.Cluster.Model
{
    public class Cluster
    {
        private static readonly string InternalName = Guid.NewGuid().ToString();
        private static volatile object _syncRoot = new object();

        public static Tuple<IClusterSnapshotControl, ILogger> ControlFor(Properties properties, string nodeName)
            => ControlFor(World.Start("vlingo-cluster"), properties, nodeName);

        public static Tuple<IClusterSnapshotControl, ILogger> ControlFor(World world, Properties properties, string nodeName)
        {
            lock (_syncRoot)
            {
                if (IsRunningInside(world))
                {
                    throw new InvalidOperationException($"Cluster is already running inside World: {world.Name}");
                }

                var (control, logger) = ClusterSnapshotControlFactory.Instance(world, nodeName);
    
                world.RegisterDynamic(InternalName, control);
                
                return new Tuple<IClusterSnapshotControl, ILogger>(control, logger);   
            }
        }

        public static bool IsRunningInside(World world) => world.ResolveDynamic<IClusterSnapshotControl>(InternalName) != null;
    }
}