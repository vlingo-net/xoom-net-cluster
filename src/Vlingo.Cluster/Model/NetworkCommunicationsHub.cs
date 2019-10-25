// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Actors;
using Vlingo.Cluster.Model.Outbound;
using Vlingo.Wire.Fdx.Inbound;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model
{
    internal class NetworkCommunicationsHub : ICommunicationsHub
    {
        private const string AppName = "APP";
        private const string OpName = "OP";
        
        private IInboundStream? _applicationInboundStream;
        private IApplicationOutboundStream? _applicationOutboundStream;
        private IInboundStream? _operationalInboundStream;
        private IOperationalOutboundStream? _operationalOutboundStream;
        
        public void Close()
        {
            _operationalInboundStream?.Stop();
            _operationalOutboundStream?.Stop();
            _applicationInboundStream?.Stop();
            _applicationOutboundStream?.Stop();
        }

        public void Open(Stage stage, Wire.Node.Node node, IInboundStreamInterest interest, IConfiguration configuration)
        {
            _operationalInboundStream =
                InboundStreamFactory.Instance(
                    stage,
                    interest,
                    node.OperationalAddress.Port,
                    AddressType.Op,
                    OpName,
                    Properties.Instance.OperationalBufferSize(),
                    Properties.Instance.OperationalInboundProbeInterval());
            
            _operationalOutboundStream =
                OperationalOutboundStreamFactory.Instance(
                    stage,
                    node,
                    new ManagedOutboundSocketChannelProvider(node, AddressType.Op, configuration),
                    new ByteBufferPool(
                        Properties.Instance.OperationalOutgoingPooledBuffers(),
                        Properties.Instance.OperationalBufferSize()));
            
            _applicationInboundStream =
                InboundStreamFactory.Instance(
                    stage,
                    interest,
                    node.ApplicationAddress.Port,
                    AddressType.App,
                    AppName,
                    Properties.Instance.ApplicationBufferSize(),
                    Properties.Instance.ApplicationInboundProbeInterval());
            
            _applicationOutboundStream =
                ApplicationOutboundStreamFactory.Instance(
                    stage,
                    new ManagedOutboundSocketChannelProvider(node, AddressType.App, configuration),
                    new ByteBufferPool(
                        Properties.Instance.ApplicationOutgoingPooledBuffers(),
                        Properties.Instance.ApplicationBufferSize()));
        }

        public IInboundStream? ApplicationInboundStream => _applicationInboundStream;

        public IApplicationOutboundStream? ApplicationOutboundStream => _applicationOutboundStream;

        public IInboundStream? OperationalInboundStream => _operationalInboundStream;

        public IOperationalOutboundStream? OperationalOutboundStream => _operationalOutboundStream;
        
        public void Start()
        {
        }
    }
}