// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.IO;
using Vlingo.Xoom.Wire.Message;

namespace Vlingo.Cluster.Model.Message
{
    using Xoom.Wire.Nodes;

    public class OperationalMessageCache
    {
        private readonly Dictionary<string, RawMessage> _messages;
        private readonly Node _node;

        public OperationalMessageCache(Node node)
        {
            _messages = new Dictionary<string, RawMessage>();
            _node = node;

            CacheValidTypes();
        }

        public RawMessage CachedRawMessage(string type)
        {
            var rawMessage = _messages[type];

            if (rawMessage == null)
            {
                throw new ArgumentNullException($"Cache does not support type: '{type}'");
            }

            return rawMessage;
        }

        private void CacheValidTypes()
        {
            var buffer = new MemoryStream(1000);
            
            CacheElect(buffer);
            CacheJoin(buffer);
            CacheLeader(buffer);
            CacheLeave(buffer);
            CachePing(buffer);
            CachePulse(buffer);
            CacheVote(buffer);
        }
        
        private void CacheElect(MemoryStream buffer)
        {
            MessageConverters.MessageToBytes(new Elect(_node.Id), buffer);
            CacheMessagePair(buffer, OperationalMessage.ELECT);
        }
        
        private void CacheJoin(MemoryStream buffer)
        {
            MessageConverters.MessageToBytes(new Join(_node), buffer);
            CacheMessagePair(buffer, OperationalMessage.JOIN);
        }

        private void CacheLeader(MemoryStream buffer)
        {
            MessageConverters.MessageToBytes(new Leader(_node.Id), buffer);
            CacheMessagePair(buffer, OperationalMessage.LEADER);
        }

        private void CacheLeave(MemoryStream buffer)
        {
            MessageConverters.MessageToBytes(new Leave(_node.Id), buffer);
            CacheMessagePair(buffer, OperationalMessage.LEAVE);
        }

        private void CachePing(MemoryStream buffer)
        {
            MessageConverters.MessageToBytes(new Ping(_node.Id), buffer);
            CacheMessagePair(buffer, OperationalMessage.PING);
        }

        private void CachePulse(MemoryStream buffer)
        {
            MessageConverters.MessageToBytes(new Pulse(_node.Id), buffer);
            CacheMessagePair(buffer, OperationalMessage.PULSE);
        }

        private void CacheVote(MemoryStream buffer)
        {
            MessageConverters.MessageToBytes(new Vote(_node.Id), buffer);
            CacheMessagePair(buffer, OperationalMessage.VOTE);
        }

        private void CacheMessagePair(MemoryStream buffer, string typeKey)
        {
            var cachedMessage = _node.Id.Value.ToRawMessage(buffer);
            _messages.Add(typeKey, cachedMessage);
        }
    }
}