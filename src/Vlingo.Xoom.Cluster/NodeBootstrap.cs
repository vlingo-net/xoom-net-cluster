// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq.Expressions;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model;
using Vlingo.Xoom.Wire.Nodes;
using Properties = Vlingo.Xoom.Cluster.Model.Properties;

namespace Vlingo.Xoom.Cluster;

public sealed class NodeBootstrap
{
    private readonly Tuple<IClusterSnapshotControl, ILogger> _clusterSnapshotControl;

    public static void Main(string[] args)
    {
    }
        
    public static NodeBootstrap Boot<TActor>(string nodeName) => Boot<TActor>(nodeName, false);
        
    public static NodeBootstrap Boot<TActor>(string nodeName, bool embedded) => Boot<TActor>(World.Start("xoom-cluster"), nodeName, embedded);
        
    public static NodeBootstrap Boot<TActor>(World world, string nodeName, bool embedded)
        => Boot<TActor>(World.Start("xoom-cluster"), node => default!, Properties.Instance, nodeName, embedded);

    public static NodeBootstrap Boot<TActor>(
        World world,
        Expression<Func<Node, TActor>> instantiator,
        Properties properties,
        string nodeName,
        bool embedded)
    {
        Properties.Instance.ValidateRequired(nodeName);
  
        var control = Model.Cluster.ControlFor(world, instantiator, properties, nodeName);
  
        var instance = new NodeBootstrap(control, nodeName);
  
        control.Item2.Info($"Successfully started cluster node: '{nodeName}'");
  
        if (!embedded)
        {
            control.Item2.Info("==========");
        }

        return instance;
    }

    public IClusterSnapshotControl ClusterSnapshotControl => _clusterSnapshotControl.Item1;

    private NodeBootstrap(Tuple<IClusterSnapshotControl, ILogger> control, string nodeName)
    {
        _clusterSnapshotControl = control;
            
        var shutdownHook = new ShutdownHook(nodeName, control);
        shutdownHook.Register();
    }
}