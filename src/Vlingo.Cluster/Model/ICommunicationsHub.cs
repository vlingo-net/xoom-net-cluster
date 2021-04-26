// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model.Outbound;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Wire.Fdx.Inbound;
using Vlingo.Xoom.Wire.Fdx.Outbound;
using Vlingo.Xoom.Wire.Node;

namespace Vlingo.Cluster.Model
{
    public interface ICommunicationsHub
    {
        void Close();

        void Open(Stage stage, Xoom.Wire.Node.Node node, IInboundStreamInterest interest, IConfiguration configuration);

        IInboundStream? ApplicationInboundStream { get; }

        IApplicationOutboundStream? ApplicationOutboundStream { get; }

        IInboundStream? OperationalInboundStream { get; }
        
        IOperationalOutboundStream? OperationalOutboundStream { get; }

        void Start();
    }
}