// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using Vlingo.Actors;

namespace Vlingo.Cluster.Model
{
    public class Cluster
    {
        private static IClusterSnapshotControl? _control;
        private static World? _world;
        
        private static volatile object _syncRoot = new object();

        public static Tuple<IClusterSnapshotControl, ILogger> ControlFor(string name)
        {
            if (_world != null)
            {
                throw new InvalidOperationException("Cluster snapshot control already exists.");
            }
            
            return ControlFor(World.Start("vlingo-cluster"), name);
        }

        public static Tuple<IClusterSnapshotControl, ILogger> ControlFor(World world, string name)
        {
            lock (_syncRoot)
            {
                if (_control != null)
                {
                    throw new InvalidOperationException("Cluster snapshot control already exists.");
                }

                _world = world;

                var control = ClusterSnapshotControlFactory.Instance(world, name);
    
                _control = control.Item1;
    
                return control;   
            }
        }

        public static void Reset()
        {
            lock (_syncRoot)
            {
                _control = null;
            }
        }

        public static bool IsRunning(bool expected, int retries)
        {
            for (int idx = 0; idx < retries; ++idx)
            {
                if (IsRunning() == expected)
                {
                    return expected;
                }

                try
                {
                    Thread.Sleep(500);
                }
                catch
                {
                }
            }
            
            return !expected;
        }

        public static bool IsRunning() => _control != null;
    }
}