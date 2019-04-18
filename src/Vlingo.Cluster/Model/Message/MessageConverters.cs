// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.IO;
using System.Text;
using Vlingo.Wire.Channel;
using Vlingo.Wire.Message;

namespace Vlingo.Cluster.Model.Message
{
    public static class MessageConverters
    {
        public static void MessageToBytes(ApplicationSays app, MemoryStream buffer)
        {
            var builder = new StringBuilder(OperationalMessage.APP).Append("\n");
            
            builder
                .Append("id=").Append(app.Id.Value)
                .Append(" nm=").Append(app.Name.Value)
                .Append(" si=").Append(app.SaysId)
                .Append("\n").Append(app.Payload);
            
            var bytes = Converters.TextToBytes(builder.ToString());
            
            buffer.Write(bytes, 0, bytes.Length);
        }

        public static void MessageToBytes(Directory dir, MemoryStream buffer)
        {
            var builder = new StringBuilder(OperationalMessage.DIR).Append("\n");
            
            builder.Append("id=").Append(dir.Id.Value).Append(" nm=").Append(dir.Name.Value).Append("\n");
            
            var lf = string.Empty;

            foreach (var node in dir.Nodes)
            {
                builder
                    .Append(lf)
                    .Append("id=").Append(node.Id.Value)
                    .Append(" nm=").Append(node.Name.Value)
                    .Append(" op=").Append(node.OperationalAddress.HostName)
                    .Append(":").Append(node.OperationalAddress.Port)
                    .Append(" app=").Append(node.ApplicationAddress.HostName)
                    .Append(":").Append(node.ApplicationAddress.Port);

                lf = "\n";
            }
            
            var bytes = Converters.TextToBytes(builder.ToString());
            
            buffer.Write(bytes, 0, bytes.Length);
        }

        public static void MessageToBytes(Elect elect, MemoryStream buffer) => BasicMessageToBytes(elect, OperationalMessage.ELECT, buffer);

        public static void MessageToBytes(Join join, MemoryStream buffer)
        {
            var builder =
                new StringBuilder(OperationalMessage.JOIN)
                    .Append("\n")
                    .Append("id=")
                    .Append(join.Node.Id.Value)
                    .Append(" nm=")
                    .Append(join.Node.Name.Value)
                    .Append(" op=")
                    .Append(join.Node.OperationalAddress.HostName)
                    .Append(":")
                    .Append(join.Node.OperationalAddress.Port)
                    .Append(" app=")
                    .Append(join.Node.ApplicationAddress.HostName)
                    .Append(":")
                    .Append(join.Node.ApplicationAddress.Port);
            
            var bytes = Converters.TextToBytes(builder.ToString());
            
            buffer.Write(bytes, 0, bytes.Length);
        }

        public static void MessageToBytes(Leader leader, MemoryStream buffer) => BasicMessageToBytes(leader, OperationalMessage.LEADER, buffer);
        
        public static void MessageToBytes(Leave leave, MemoryStream buffer) => BasicMessageToBytes(leave, OperationalMessage.LEAVE, buffer);
        
        public static void MessageToBytes(Ping ping, MemoryStream buffer) => BasicMessageToBytes(ping, OperationalMessage.PING, buffer);
        
        public static void MessageToBytes(Pulse pulse, MemoryStream buffer) => BasicMessageToBytes(pulse, OperationalMessage.PULSE, buffer);
        
        public static void MessageToBytes(Split split, MemoryStream buffer) => BasicMessageToBytes(split, OperationalMessage.SPLIT, buffer);
        
        public static void MessageToBytes(Vote vote, MemoryStream buffer) => BasicMessageToBytes(vote, OperationalMessage.VOTE, buffer);

        private static void BasicMessageToBytes(OperationalMessage message, string type, MemoryStream buffer)
        {
            var builder =
                new StringBuilder(type)
                    .Append("\n")
                    .Append("id=")
                    .Append(message.Id.Value);
            
            var bytes = Converters.TextToBytes(builder.ToString());

            buffer.Clear();
            buffer.Write(bytes, 0, bytes.Length);
        }
    }
}