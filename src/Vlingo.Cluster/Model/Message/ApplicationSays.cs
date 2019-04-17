// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Message
{
    public class ApplicationSays : OperationalMessage
    {
        public static ApplicationSays From(string content)
        {
            return null;
        }
        
        public Name Name { get; }
        
        public string Payload { get; }
        
        public string SaysId { get; }
        
        private ApplicationSays(Id id, Name name, string payload) : base(id)
        {
            Name = name;
            Payload = payload;
            SaysId = Guid.NewGuid().ToString();
        }
        
        private ApplicationSays(Id id, Name name, string saysId, string payload) : base(id)
        {
            Name = name;
            SaysId = saysId;
            Payload = payload;
        }
    }
}