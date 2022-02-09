// Copyright © 2012-2022 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Threading;
using Vlingo.Xoom.Actors;
using Vlingo.Xoom.Cluster.Model.Application;
using Vlingo.Xoom.Cluster.Model.Attribute;
using Vlingo.Xoom.Cluster.Model.Message;
using Vlingo.Xoom.Cluster.Model.Nodes;
using Vlingo.Xoom.Wire.Fdx.Inbound;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model
{   
    public sealed class ClusterSnapshotActor : Actor, IClusterSnapshot, IClusterSnapshotControl, IInboundStreamInterest, IRegistryInterest
    {
        private readonly IAttributesAgent _attributesAgent;
        private readonly IClusterApplication _clusterApplication;
        private readonly ClusterApplicationBroadcaster _broadcaster;
        private readonly ICommunicationsHub _communicationsHub;
        private readonly ILocalLiveNode _localLiveNode;
        private readonly Node _localNode;

        public ClusterSnapshotActor(ClusterSnapshotInitializer initializer, IClusterApplication clusterApplication)
        {
            _broadcaster = new ClusterApplicationBroadcaster(Logger);
            _communicationsHub = initializer.CommunicationsHub;
            _communicationsHub.Open(Stage, initializer.LocalNode, SelfAs<IInboundStreamInterest>(), initializer.Configuration);
            _localNode = initializer.LocalNode;
            _clusterApplication = clusterApplication;
            _broadcaster.RegisterClusterApplication(clusterApplication);
            
            _clusterApplication.Start();
            _clusterApplication.InformResponder(_communicationsHub.ApplicationOutboundStream);
            
            initializer.Registry.RegisterRegistryInterest(SelfAs<IRegistryInterest>());

            _attributesAgent = AttributesAgentFactory.Instance(Stage, _localNode, _broadcaster,
                _communicationsHub.OperationalOutboundStream!, initializer.Configuration);
            
            _localLiveNode = LocalLiveNodeFactory.Instance(Stage, _localNode, SelfAs<IClusterSnapshot>(),
                initializer.Registry, _communicationsHub.OperationalOutboundStream!, initializer.Configuration);
            
            _localLiveNode.RegisterNodeSynchronizer(_attributesAgent);
            
            _communicationsHub.Start();
        }
        
        //=========================================
        // ClusterSnapshot
        //=========================================

        public void QuorumAchieved() => _broadcaster.InformQuorumAchieved();

        public void QuorumLost() => _broadcaster.InformQuorumLost();

        //=========================================
        // ClusterSnapshotControl
        //=========================================
        
        public void ShutDown()
        {
            if (IsStopped)
            {
                return;
            }

            _localLiveNode.Stop();
            _clusterApplication.Stop();
            _attributesAgent.Stop();
            Pause();
            _communicationsHub.Close();
            Stop();
            Stage.World.Terminate();
        }
        
        //=========================================
        // InboundStreamInterest (operations and application)
        //=========================================

        public void HandleInboundStreamMessage(AddressType addressType, RawMessage message)
        {
            if (IsStopped)
            {
                return;
            }
            
            if (addressType.IsOperational)
            {
                var textMessage = message.AsTextMessage();
                var typedMessage = OperationalMessage.MessageFrom(textMessage);
                if (typedMessage != null)
                {
                    if (typedMessage.IsApp)
                    {
                        _attributesAgent.HandleInboundStreamMessage(addressType, message);   
                    }
                    else
                    {
                        _localLiveNode.Handle(typedMessage);
                    }
                }
                else
                {
                    Logger.Warn($"ClusterSnapshot received invalid raw message '{textMessage}'");
                }
            }
            else if (addressType.IsApplication)
            {
                _clusterApplication.HandleApplicationMessage(message); // TODO
            }
            else
            {
                Logger.Warn($"ClusterSnapshot couldn't dispatch incoming message; unknown address type: {addressType} for message: {message.AsTextMessage()}");
            }
        }
        
        //=========================================
        // RegistryInterest
        //=========================================

        public void InformAllLiveNodes(IEnumerable<Node> liveNodes, bool isHealthyCluster) =>
            _broadcaster.InformAllLiveNodes(liveNodes, isHealthyCluster);

        public void InformConfirmedByLeader(Node node, bool isHealthyCluster) =>
            _broadcaster.InformNodeIsHealthy(node.Id, isHealthyCluster);

        public void InformCurrentLeader(Node node, bool isHealthyCluster) =>
            _broadcaster.InformLeaderElected(node.Id, isHealthyCluster, node.Id.Equals(_localNode.Id));

        public void InformMergedAllDirectoryEntries(IEnumerable<Node> liveNodes, IEnumerable<MergeResult> mergeResults, bool isHealthyCluster)
        {
            foreach (var mergeResult in mergeResults)
            {
                if (mergeResult.Left)
                {
                    _broadcaster.InformNodeLeftCluster(mergeResult.Node.Id, isHealthyCluster);
                }
                else if (mergeResult.Joined)
                {
                    _broadcaster.InformNodeJoinedCluster(mergeResult.Node.Id, isHealthyCluster);
                }
            }
        }

        public void InformLeaderDemoted(Node node, bool isHealthyCluster) =>
            _broadcaster.InformLeaderLost(node.Id, isHealthyCluster);

        public void InformNodeIsHealthy(Node node, bool isHealthyCluster) =>
            _broadcaster.InformNodeIsHealthy(node.Id, isHealthyCluster);

        public void InformNodeJoinedCluster(Node node, bool isHealthyCluster) =>
            _broadcaster.InformNodeJoinedCluster(node.Id, isHealthyCluster);

        public void InformNodeLeftCluster(Node node, bool isHealthyCluster) =>
            _broadcaster.InformNodeLeftCluster(node.Id, isHealthyCluster);

        public void InformNodeTimedOut(Node node, bool isHealthyCluster) =>
            _broadcaster.InformNodeLeftCluster(node.Id, isHealthyCluster);
        
        private void Pause()
        {
            try
            {
                Thread.Sleep(3000);
            }
            catch 
            {
                // ignore
            }
        }
    }
}