// Copyright © 2012-2020 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.IO;
using System.Text;
using Vlingo.Cluster.Model.Message;
using Vlingo.Common;
using Vlingo.Wire.Channel;
using Vlingo.Wire.Message;

namespace Vlingo.Cluster.Tests.Model.Message
{
    public class MessageFixtures
    {
        private static AtomicInteger _nextPortNumber = new AtomicInteger(27270);
        
        public static short DefaultNodeId => 1;
        
        public static string DefaultNodeName => "node1";
        public static string[] OpAddresses => new [] { "", $"localhost:{_nextPortNumber.IncrementAndGet()}", $"localhost:{_nextPortNumber.IncrementAndGet()}", $"localhost:{_nextPortNumber.IncrementAndGet()}" };
        
        public static string[] AppAddresses => new [] { "", $"localhost:{_nextPortNumber.IncrementAndGet()}", $"localhost:{_nextPortNumber.IncrementAndGet()}", $"localhost:{_nextPortNumber.IncrementAndGet()}" };
        

        public static string DirectoryAsText(int id1, int id2, int id3)
        {
            var builder =
                new StringBuilder(OperationalMessage.DIR)
                    .Append("\n")
                    .Append("id=").Append(id1).Append(" nm=node").Append(id1).Append("\n")
                    .Append("id=").Append(id1).Append(" nm=node").Append(id1).Append(" op=").Append(OpAddresses[1]).Append(" app=").Append(AppAddresses[1]).Append("\n")
                    .Append("id=").Append(id2).Append(" nm=node").Append(id2).Append(" op=").Append(OpAddresses[2]).Append(" app=").Append(AppAddresses[2]).Append("\n")
                    .Append("id=").Append(id3).Append(" nm=node").Append(id3).Append(" op=").Append(OpAddresses[3]).Append(" app=").Append(AppAddresses[3]);

            return builder.ToString();
        }

        public static string JoinAsText() =>
            $"{OperationalMessage.JOIN}\nid=1 nm=node1 op={OpAddresses[1]} app={AppAddresses[1]}";
        
        public static string LeaderAsText() => $"{OperationalMessage.LEADER}\nid=1";
        
        public static string LeaveAsText() => $"{OperationalMessage.LEAVE}\nid=1";

        public static MemoryStream BytesFrom(string text)
        {
            var message = new RawMessage(Converters.TextToBytes(text));
            var header = RawMessageHeader.From(1, 0, message.Length);
            message.Header(header);
    
            var buffer = new MemoryStream(4000);
            message.CopyBytesTo(buffer);
            buffer.Flip();
    
            return buffer;
        }
    }
}