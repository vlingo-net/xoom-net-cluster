// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Text;
using Vlingo.Cluster.Model.Message;
using Vlingo.Wire.Message;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Attribute.Message
{
    public sealed class ReceivedAttributeMessage
    {
        private const string SourceNodeIdKey = "sourceNodeIdKey";
        private const string SourceNodeNameKey = "sourceNodeNameKey";
        private const string SourceNodeOpPortKey = "sourceNodeOpPortKey";
        private const string SourceNodeAppPortKey = "sourceNodeAppPortKey";
  
        private const string ClassOfMessageKey = "classOfMessage";
        private const string CorrelatingMessageIdKey = "correlatingMessageId";
        private const string MessageTypeKey = "type";
        private const string TrackingIdKey = "trackingId";
  
        private const string AttributeSetNameKey = "attributeSetName";
  
        private const string AttributeNameKey = "attributeName";
        private const string AttributeTypeKey = "attributeType";
        private const string AttributeValueKey = "attributeValue";

        private readonly Dictionary<string, string> _payloadMap;

        public ReceivedAttributeMessage(RawMessage message)
        {
            _payloadMap = ParsePayload(message);
        }

        public Attribute<string> Attribute()
        {
            Enum.TryParse<AttributeType>(AttributeType, out var type);
            return Attribute<string>.From(AttributeName, type, AttributeValue);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("ReceivedAttributeMessage[payloadMap=");
            foreach (var pairs in _payloadMap)
            {
                builder.AppendLine($"{pairs.Key}, {pairs.Value}");
            }

            builder.Append("]");

            return builder.ToString();
        }

        public Id SourceNodeId => Id.Of(int.Parse(_payloadMap[SourceNodeIdKey]));
        
        public Name SourceNodeName => Name.Of(_payloadMap[SourceNodeNameKey]);
        
        public Id SourceNodeOpPort => Id.Of(int.Parse(_payloadMap[SourceNodeOpPortKey]));
        
        public Id SourceNodeAppPort => Id.Of(int.Parse(_payloadMap[SourceNodeAppPortKey]));
        
        public string ClassOfMessage => _payloadMap[ClassOfMessageKey];
        
        public string CorrelatingMessageId => _payloadMap[CorrelatingMessageIdKey];
        
        public string TrackingId => _payloadMap[TrackingIdKey];

        public ApplicationMessageType Type
        {
            get
            {
                Enum.TryParse<ApplicationMessageType>(_payloadMap[MessageTypeKey], out var result);
                return result;
            }
        }
        
        public string AttributeSetName => _payloadMap[AttributeSetNameKey];
        
        public string AttributeName => _payloadMap[AttributeNameKey];
        
        public string AttributeType => _payloadMap[AttributeTypeKey];
        
        public string AttributeValue => _payloadMap[AttributeValueKey];
        
        private Dictionary<string,string> ParsePayload(RawMessage message)
        {
            var map = new Dictionary<string, string>();
    
            var says = ApplicationSays.From(message.AsTextMessage());
    
            map.Add(SourceNodeIdKey, says.Id.ValueString());
            map.Add(SourceNodeNameKey, says.Name.Value);
    
            var parsed = says.Payload.Split('\n');
    
            map.Add(ClassOfMessageKey, parsed[0]);
    
            switch (parsed[0])
            {
                case "ConfirmCreateAttributeSet":
                    map.Add(CorrelatingMessageIdKey, parsed[1]);
                    map.Add(TrackingIdKey, parsed[2]);
                    map.Add(MessageTypeKey, parsed[3]);
                    map.Add(AttributeSetNameKey, parsed[4]);
                    break;
                case "ConfirmRemoveAttributeSet":
                    map.Add(CorrelatingMessageIdKey, parsed[1]);
                    map.Add(TrackingIdKey, parsed[2]);
                    map.Add(MessageTypeKey, parsed[3]);
                    map.Add(AttributeSetNameKey, parsed[4]);
                    break;
                case "ConfirmAttribute":
                    map.Add(CorrelatingMessageIdKey, parsed[1]);
                    map.Add(TrackingIdKey, parsed[2]);
                    map.Add(MessageTypeKey, parsed[3]);
                    map.Add(AttributeSetNameKey, parsed[4]);
                    map.Add(AttributeNameKey, parsed[5]);
                    break;
                case "CreateAttributeSet":
                    map.Add(TrackingIdKey, parsed[1]);
                    map.Add(MessageTypeKey, parsed[2]);
                    map.Add(AttributeSetNameKey, parsed[3]);
                    break;
                case "RemoveAttributeSet":
                    map.Add(TrackingIdKey, parsed[1]);
                    map.Add(MessageTypeKey, parsed[2]);
                    map.Add(AttributeSetNameKey, parsed[3]);
                    break;
                case "AddAttribute":
                case "RemoveAttribute":
                case "ReplaceAttribute":
                    map.Add(CorrelatingMessageIdKey, parsed[1]);
                    map.Add(TrackingIdKey, parsed[2]);
                    map.Add(MessageTypeKey, parsed[3]);
                    map.Add(AttributeSetNameKey, parsed[4]);
                    map.Add(AttributeNameKey, parsed[5]);
                    map.Add(AttributeTypeKey, parsed[6]);
                    map.Add(AttributeValueKey, parsed[7]);
                    break;
            }
    
            return map;
        }
    }
}