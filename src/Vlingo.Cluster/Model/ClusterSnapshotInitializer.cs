// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Actors;
using Vlingo.Cluster.Model.Node;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model
{
    internal class ClusterSnapshotInitializer
    {
        private readonly ICommunicationsHub _communicationsHub;
        private readonly IConfiguration _configuration;
        private readonly Wire.Node.Node _localNode;
        private readonly Id _localNodeId;
        private readonly IRegistry _registry;

        internal ClusterSnapshotInitializer(string nodeNameText, Properties properties, ILogger logger)
        {
            _localNodeId = Id.Of(properties.NodeId(nodeNameText));
            _configuration = new ClusterConfiguration(logger);
            _localNode = _configuration.NodeMatching(_localNodeId);
            _communicationsHub = new NetworkCommunicationsHub();
            _registry = new LocalRegistry(_localNode, _configuration, logger);
        }

        internal ICommunicationsHub CommunicationsHub => _communicationsHub;

        internal IConfiguration Configuration => _configuration;

        public Wire.Node.Node LocalNode => _localNode;

        public Id LocalNodeId => _localNodeId;

        public IRegistry Registry => _registry;
    }
}