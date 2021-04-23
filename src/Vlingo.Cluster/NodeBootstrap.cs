// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Actors;

namespace Vlingo.Cluster
{
    using Model;
    
    public sealed class NodeBootstrap
    {
        private readonly (IClusterSnapshotControl, ILogger) _clusterSnapshotControl;

        public static void Main(string[] args)
        {
        }
        
        public static NodeBootstrap Boot(string nodeName) => Boot(nodeName, false);

        public static NodeBootstrap Boot(string nodeName, bool embedded)
        {
            Properties.Instance.ValidateRequired(nodeName);
  
            var control = Cluster.ControlFor(nodeName);
  
            var instance = new NodeBootstrap(control, nodeName);
  
            control.Item2.Info($"Successfully started cluster node: '{nodeName}'");
  
            if (!embedded)
            {
                control.Item2.Info("==========");
            }

            return instance;
        }

        public IClusterSnapshotControl ClusterSnapshotControl => _clusterSnapshotControl.Item1;

        private NodeBootstrap((IClusterSnapshotControl, ILogger) control, string nodeName)
        {
            _clusterSnapshotControl = control;
            
            var shutdownHook = new ShutdownHook(nodeName, control);
            shutdownHook.Register();
        }
    }
}