// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System.IO;
using Vlingo.Xoom.Cluster.Model;
using Vlingo.Xoom.Actors.Plugin.Logging.Console;
using Vlingo.Xoom.Wire.Channel;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Tests.Model
{
    public class AbstractMessageTool
    {
        protected IConfiguration Config = new ClusterConfiguration(ConsoleLogger.TestInstance());

        public RawMessage BuildRawMessageBuffer(MemoryStream buffer, string message)
        {
            buffer.Clear();
            buffer.Write(Converters.TextToBytes(message));
            buffer.Flip();
            var rawMessage = RawMessage.From(1, 0, (int)buffer.Length);
            rawMessage.Put(buffer, false);

            return rawMessage;
        }

        public MemoryStream BytesFrom(RawMessage message, MemoryStream buffer)
        {
            buffer.Clear();
            message.CopyBytesTo(buffer);
            buffer.Flip();
            return buffer;
        }
    }
}