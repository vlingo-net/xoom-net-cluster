// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq.Expressions;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model;

public class Cluster
{
    private static readonly string InternalName = Guid.NewGuid().ToString();
    private static volatile object _syncRoot = new object();

    public static Tuple<IClusterSnapshotControl, ILogger> ControlFor<TActor>(
        Expression<Func<Node, TActor>> instantiator,
        Properties properties,
        string nodeName)
        => ControlFor(World.Start("xoom-cluster"), instantiator, properties, nodeName);

    public static Tuple<IClusterSnapshotControl, ILogger> ControlFor<TActor>(
        World world,
        Expression<Func<Node, TActor>> instantiator,
        Properties properties,
        string nodeName)
    {
        lock (_syncRoot)
        {
            if (IsRunningInside(world))
            {
                throw new InvalidOperationException($"Cluster is already running inside World: {world.Name}");
            }

            var (control, logger) = ClusterSnapshotControlFactory.Instance(world, instantiator, nodeName);
    
            world.RegisterDynamic(InternalName, control);
                
            return new Tuple<IClusterSnapshotControl, ILogger>(control, logger);   
        }
    }

    public static bool IsRunningInside(World world) => world.ResolveDynamic<IClusterSnapshotControl>(InternalName) != null;
}