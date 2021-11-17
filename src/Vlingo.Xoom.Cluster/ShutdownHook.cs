// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Threading;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model;

namespace Vlingo.Xoom.Cluster
{
    internal sealed class ShutdownHook
    {
        private readonly Tuple<IClusterSnapshotControl, ILogger> _control;
        private readonly string _nodeName;

        internal ShutdownHook(string nodeName, Tuple<IClusterSnapshotControl, ILogger> control)
        {
            _nodeName = nodeName;
            _control = control;
        }

        internal void Register()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) =>
            {
                _control.Item2.Info("\n==========");
                _control.Item2.Info($"Stopping node: '{_nodeName}' ...");
                _control.Item1.ShutDown();
                Pause();
                _control.Item2.Info($"Stopped node: '{_nodeName}'");
            };
        }
        
        private void Pause()
        {
            try
            {
                Thread.Sleep(4000);
            }
            catch 
            {
                // ignore
            }
        }
    }
}