// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using Vlingo.Xoom.Wire.Nodes;

namespace Vlingo.Xoom.Cluster.Model.Message;

public sealed class ApplicationSays : OperationalMessage
{
    public static ApplicationSays From(string content)
    {
        var id = OperationalMessagePartsBuilder.IdFrom(content);
        var name = OperationalMessagePartsBuilder.NameFrom(content);
        var saysId = OperationalMessagePartsBuilder.SaysIdFrom(content);
        var payload = OperationalMessagePartsBuilder.PayloadFrom(content);
            
        return new ApplicationSays(id, name, saysId, payload);
    }
        
    public static ApplicationSays From(Id id, Name name, string? payload) => new ApplicationSays(id, name, payload);
        
    public Name Name { get; }
        
    public string? Payload { get; }
        
    public string? SaysId { get; }

    public override bool IsApp => true;

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != typeof(ApplicationSays))
        {
            return false;
        }

        var otherAppSaid = (ApplicationSays) obj;

        if (Payload == null)
        {
            return Name.Equals(otherAppSaid.Name);
        }
            
        return Name.Equals(otherAppSaid.Name) && 
               Payload.Equals(otherAppSaid.Payload);
    }

    public override int GetHashCode()
    {
        if (Payload == null)
        {
            return 31 * Name.GetHashCode();
        }
            
        return 31 * Name.GetHashCode() + Payload.GetHashCode();
    }

    public override string ToString() => $"ApplicationSays[{Id},{Name},{Payload}]";

    private ApplicationSays(Id id, Name name, string? payload) : base(id)
    {
        Name = name;
        Payload = payload;
        SaysId = Guid.NewGuid().ToString();
    }
        
    private ApplicationSays(Id id, Name name, string? saysId, string? payload) : base(id)
    {
        Name = name;
        SaysId = saysId;
        Payload = payload;
    }
}