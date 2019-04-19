// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Actors;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model
{
    internal class ClusterSnapshotInitializer
    {
        private readonly ICommunicationsHub _communicationsHub;
        private readonly IConfiguration _configuration;
        private readonly Wire.Node.Node _localNode;
        private readonly Id _localNodeId;

        public ClusterSnapshotInitializer(string name, Properties instance, ILogger worldDefaultLogger)
        {
            throw new System.NotImplementedException();
        }
    }
}