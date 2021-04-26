// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using Vlingo.Cluster.Model.Message;
using Vlingo.Xoom.Wire.Channel;
using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;
using Xunit;
using Xunit.Abstractions;

namespace Vlingo.Cluster.Tests.Model.Message
{
    public class RawMessageTest : AbstractClusterTest
    {
        [Fact]
        public void TestKnownSizeWithAppend()
        {
            var buffer = new MemoryStream(1000);
            var node1 = NextNodeWith(1);
            var join = new Join(node1);
            MessageConverters.MessageToBytes(join, buffer);
            buffer.Flip();
            var messageSize = buffer.Length;
            var message = new RawMessage(messageSize); // known size
            message.Header(RawMessageHeader.From(node1.Id.Value, 0, messageSize));
            message.Append(buffer.ToArray(), 0, messageSize);
    
            Assert.Equal(node1.Id.Value, message.Header().NodeId);
            Assert.Equal(messageSize, message.Header().Length);
            Assert.Equal(join, OperationalMessage.MessageFrom(message.AsTextMessage()));
        }

        [Fact]
        public void TestFromBytesWithLengthAndRequiredMessageLength()
        {
            var buffer = new MemoryStream(1000);
            var node1 = NextNodeWith(1);
            var join = new Join(node1);
            MessageConverters.MessageToBytes(join, buffer);
            buffer.Flip();
            var messageSize = buffer.Length;
            var messageBytes = new byte[messageSize];
            Array.Copy(buffer.ToArray(), 0, messageBytes, 0, messageSize);
            var message = new RawMessage(messageBytes);
            message.Header(RawMessageHeader.From(node1.Id.Value, 0, message.Length));
    
            Assert.Equal(node1.Id.Value, message.Header().NodeId);
            Assert.Equal(message.Length, message.Header().Length);
            Assert.Equal(message.Length, message.RequiredMessageLength);
            Assert.Equal(join, OperationalMessage.MessageFrom(message.AsTextMessage()));
        }

        [Fact]
        public void TestCopyBytesTo()
        {
            var buffer = new MemoryStream(1000);
            var node1 = NextNodeWith(1);
            var join = new Join(node1);
            MessageConverters.MessageToBytes(join, buffer);
            buffer.Flip();
            var messageSize = buffer.Length;
            var messageBytes = new byte[messageSize];
            Array.Copy(buffer.ToArray(), 0, messageBytes, 0, messageSize);
            var message = new RawMessage(messageBytes);
            message.Header(RawMessageHeader.From(node1.Id.Value, 0, message.Length));
    
            buffer.Clear();
            message.CopyBytesTo(buffer); // copyBytesTo
            var text = buffer.ToArray().BytesToText(RawMessageHeader.Bytes, (int)message.Length);
            Assert.True(OperationalMessage.MessageFrom(text).IsJoin);
        }

        [Fact]
        public void TestHeaderFrom()
        {
            var buffer = new MemoryStream(1000);
            var header = RawMessageHeader.From(1, 0, 100);
            var message = new RawMessage(100);
            header.CopyBytesTo(buffer);
            buffer.Flip();
            message.HeaderFrom(buffer);
            buffer.Flip();
            var convertedHeader = RawMessageHeader.From(buffer);
            Assert.Equal(header, convertedHeader);
        }

        [Fact]
        public void TestPut()
        {
            var buffer = new MemoryStream(1000);
            var node1 = Node.With(Id.Of(1), Name.Of("node1"), Host.Of("localhost"), 37371, 37372);
            var join = new Join(node1);
            MessageConverters.MessageToBytes(join, buffer);
            var message = new RawMessage(1000);
            message.Put(buffer);
            buffer.Position = 0;
            var textOfRawMessage = message.AsTextMessage();
            var textOfConvertedBuffer = Converters.BytesToText(buffer.ToArray(), 0, (int)buffer.Length);
            Assert.Equal(textOfConvertedBuffer, textOfRawMessage);
        }
        
        public RawMessageTest(ITestOutputHelper output) : base(output)
        {
        }
    }
}