// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Actors;
using Vlingo.Cluster.Model.Message;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Outbound
{
    using Vlingo.Wire.Fdx.Outbound;
    
    public class OperationalOutboundStreamActor : Actor, IOperationalOutboundStream
    {
        
        private readonly OperationalMessageCache _cache;
        private readonly Node _node;
        private readonly Outbound _outbound;

        public OperationalOutboundStreamActor(Node node, IManagedOutboundChannelProvider provider, ByteBufferPool byteBufferPool)
        {
            _node = node;
            _outbound = new Outbound(provider, byteBufferPool);
            _cache = new OperationalMessageCache(node);
        }
        
        //===================================
        // OperationalOutbound
        //===================================
        #region OperationalOutbound

        public void Close(Id id) => _outbound.Close(id);

        public async Task Application(ApplicationSays says, IEnumerable<Node> unconfirmedNodes)
        {
            var buffer = _outbound.PooledByteBuffer();
            MessageConverters.MessageToBytes(says, buffer.AsStream());
            
            var message = _node.Id.Value.ToRawMessage(buffer.AsStream());
            
            await _outbound.Broadcast(unconfirmedNodes, _outbound.BytesFrom(message, buffer));
        }

        public async Task Directory(IEnumerable<Node> allLiveNodes)
        {
            var dir = new Directory(_node.Id, _node.Name, allLiveNodes);
            
            var buffer = _outbound.PooledByteBuffer();
            MessageConverters.MessageToBytes(dir, buffer.AsStream());
            
            var message = _node.Id.Value.ToRawMessage(buffer.AsStream());
            
            await _outbound.Broadcast(_outbound.BytesFrom(message, buffer));
        }

        public async Task Elect(IEnumerable<Node> allGreaterNodes) =>
            await _outbound.Broadcast(allGreaterNodes, _cache.CachedRawMessage(OperationalMessage.ELECT));

        public async Task Join() => await _outbound.Broadcast(_cache.CachedRawMessage(OperationalMessage.JOIN));

        public async Task Leader() => await _outbound.Broadcast(_cache.CachedRawMessage(OperationalMessage.LEADER));

        public async Task Leader(Id id) =>
            await _outbound.SendTo(_cache.CachedRawMessage(OperationalMessage.LEADER), id);

        public async Task Leave() => await _outbound.Broadcast(_cache.CachedRawMessage(OperationalMessage.LEAVE));

        public void Open(Id id) => _outbound.Open(id);

        public async Task Ping(Id targetNodeId) => await _outbound.SendTo(_cache.CachedRawMessage(OperationalMessage.PING), targetNodeId);

        public async Task Pulse(Id targetNodeId)  => await _outbound.SendTo(_cache.CachedRawMessage(OperationalMessage.PULSE), targetNodeId);

        public async Task Pulse() => await _outbound.Broadcast(_cache.CachedRawMessage(OperationalMessage.PULSE));

        public async Task Split(Id targetNodeId, Id currentLeaderId)
        {
            var split = new Split(currentLeaderId);
            
            var buffer = _outbound.PooledByteBuffer();
            MessageConverters.MessageToBytes(split, buffer.AsStream());
            
            var message = _node.Id.Value.ToRawMessage(buffer.AsStream());
            
            await _outbound.SendTo(_outbound.BytesFrom(message, buffer), targetNodeId);
        }
        
        public async Task Vote(Id targetNodeId) => await _outbound.SendTo(_cache.CachedRawMessage(OperationalMessage.VOTE), targetNodeId);    
        
        #endregion

        //===================================
        // Stoppable
        //===================================
        #region Stoppable

        public override void Stop()
        {
            _outbound.Close();
            
            base.Stop();
        }

        #endregion

    }
}