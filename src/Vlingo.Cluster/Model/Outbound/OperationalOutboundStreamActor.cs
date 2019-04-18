// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Outbound
{
    public class OperationalOutboundStreamActor : Actor, IOperationalOutboundStream
    {
        private readonly OperationalMessageCache _cache;
        
        public void Close(Id id)
        {
            throw new System.NotImplementedException();
        }

        public void Application(ApplicationSays says, IEnumerable<Node> unconfirmedNodes)
        {
            throw new System.NotImplementedException();
        }

        public void Directory(IEnumerable<Node> allLiveNodes)
        {
            throw new System.NotImplementedException();
        }

        public void Elect(IEnumerable<Node> allGreaterNodes)
        {
            throw new System.NotImplementedException();
        }

        public void Join()
        {
            throw new System.NotImplementedException();
        }

        public void Leader()
        {
            throw new System.NotImplementedException();
        }

        public void Leader(Id id)
        {
            throw new System.NotImplementedException();
        }

        public void Leave()
        {
            throw new System.NotImplementedException();
        }

        public void Open(Id id)
        {
            throw new System.NotImplementedException();
        }

        public void Ping(Id targetNodeId)
        {
            throw new System.NotImplementedException();
        }

        public void Pulse(Id targetNodeId)
        {
            throw new System.NotImplementedException();
        }

        public void Pulse()
        {
            throw new System.NotImplementedException();
        }

        public void Split(Id targetNodeId, Id currentLeaderId)
        {
            throw new System.NotImplementedException();
        }

        public void Vote(Id targetNodeId)
        {
            throw new System.NotImplementedException();
        }
    }
}