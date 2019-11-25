// Copyright © 2012-2020 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Actors;

namespace Vlingo.Cluster
{
    using Model;
    
    public sealed class NodeBootstrap
    {
        private static NodeBootstrap? _instance;

        private readonly (IClusterSnapshotControl, ILogger) _clusterSnapshotControl;

        public static void Main(string[] args)
        {
            
        }
        
        public static NodeBootstrap? Boot(string nodeName)
        {
            return Boot(nodeName, false);
        }

        public static NodeBootstrap? Boot(string nodeName, bool embedded)
        {
            var mustBoot = _instance == null || !Cluster.IsRunning();
            
            if (mustBoot)
            {
                Properties.Instance.ValidateRequired(nodeName);
      
                var control = Cluster.ControlFor(nodeName);
      
                _instance = new NodeBootstrap(control, nodeName);
      
                control.Item2.Info($"Successfully started cluster node: '{nodeName}'");
      
                if (!embedded)
                {
                    control.Item2.Info("==========");
                }
            }

            return _instance;
        }

        public static NodeBootstrap? Instance => _instance;

        public IClusterSnapshotControl ClusterSnapshotControl => _clusterSnapshotControl.Item1;

        private NodeBootstrap((IClusterSnapshotControl, ILogger) control, string nodeName)
        {
            _clusterSnapshotControl = control;
            
            var shutdownHook = new ShutdownHook(nodeName, control);
            shutdownHook.Register();
        }
    }
}