// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Vlingo.Actors.TestKit;
using Vlingo.Wire.Fdx.Outbound;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Tests.Model.Outbound
{
    public class MockManagedOutboundChannel : IManagedOutboundChannel
    {
        public MockManagedOutboundChannel(Id id)
        {
            Id = id;
            Writes = new List<string>();
            Until = TestUntil.Happenings(0);
        }

        public void Close() => Writes.Clear();

        public Task Write(Stream buffer)
        {
            var message = RawMessage.ReadFromWithHeader(buffer);
            var textMessage = message.AsTextMessage();
            Writes.Add(textMessage);
            Until.Happened();
            return Task.CompletedTask;
        }
        
        public Id Id { get; }
        
        public List<string> Writes { get; }
        
        public TestUntil Until { get; set; }
    }
}