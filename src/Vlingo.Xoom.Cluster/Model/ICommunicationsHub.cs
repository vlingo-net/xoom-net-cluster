// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model.Outbound;
using Vlingo.Xoom.Wire.Fdx.Inbound;
using Vlingo.Xoom.Wire.Fdx.Outbound;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model;

public interface ICommunicationsHub
{
    void Close();

    void Open(Stage stage, Node node, IInboundStreamInterest interest, IConfiguration configuration);

    IInboundStream? ApplicationInboundStream { get; }

    IApplicationOutboundStream? ApplicationOutboundStream { get; }

    IInboundStream? OperationalInboundStream { get; }
        
    IOperationalOutboundStream? OperationalOutboundStream { get; }

    void Start();
}