// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Wire.Node;

namespace Vlingo.Cluster.Model.Message
{
    public class Vote : OperationalMessage
    {
        public static Vote From(string content)
        {
            return null;
        }
        
        public Vote(Id id) : base(id)
        {
        }
    }
}