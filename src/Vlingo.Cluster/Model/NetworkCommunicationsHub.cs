// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Cluster.Model.Outbound;
using Vlingo.Xoom.Common.Pool;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Wire.Fdx.Inbound;
using Vlingo.Xoom.Wire.Fdx.Outbound;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Node;

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

        public void Open(Stage stage, Xoom.Wire.Node.Node node, IInboundStreamInterest interest, IConfiguration configuration)
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
                    new ConsumerByteBufferPool(
                        ElasticResourcePool<IConsumerByteBuffer, string>.Config.Of(Properties.Instance.ApplicationOutgoingPooledBuffers()), 
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
                    new ConsumerByteBufferPool(
                        ElasticResourcePool<IConsumerByteBuffer, string>.Config.Of(Properties.Instance.ApplicationOutgoingPooledBuffers()), 
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