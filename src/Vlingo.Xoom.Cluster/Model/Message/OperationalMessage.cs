// Copyright © 2012-2021 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Wire.Message;
using Vlingo.Xoom.Wire.Nodes;

// ReSharper disable InconsistentNaming

namespace Vlingo.Xoom.Cluster.Model.Message
{    
    public abstract class OperationalMessage : IMessage
    {
        /// <summary>
        /// APP&lt;lf&gt;id=x nm=name si=y&lt;lf&gt;...
        /// </summary>
        public const string APP = "APP";
        
        /// <summary>
        /// CHECKHEALTH&lt;lf&gt;id=x ***internal only*** to check health
        /// </summary>
        public const  string CHECKHEALTH = "CHECKHEALTH";

        /// <summary>
        /// DIR&lt;lf&gt;id=x nm=name&lt;lf&gt;addr=...&lt;lf&gt;...
        /// </summary>
        public const  string DIR = "DIR";

        /// <summary>
        /// ELECT&lt;lf&gt;id=x an election is required
        /// </summary>
        public const  string ELECT = "ELECT";

        /// <summary>
        /// JOIN&lt;lf&gt;addr=... node joining cluster
        /// </summary>
        public const  string JOIN = "JOIN";

        /// <summary>
        /// LEADER&lt;lf&gt;id=x declare self as leader (highest existing node)
        /// </summary>
        public const  string LEADER = "LEADER";

        /// <summary>
        /// LEAVE&lt;lf&gt;id=x announce that node is leaving the cluster
        /// </summary>
        public const  string LEAVE = "LEAVE";

        /// <summary>
        /// PING&lt;lf&gt;id=x leader sends ping to follower to determine if it is alive
        /// </summary>
        public const  string PING = "PING";

        /// <summary>
        /// PULSE&lt;lf&gt;id=x follower sends pulse to leader
        /// </summary>
        public const  string PULSE = "PULSE";

        /// <summary>
        /// SPLIT&lt;lf&gt;id=x inform receiving node that it is split from known leader
        /// </summary>
        public const  string SPLIT = "SPLIT";

        /// <summary>
        /// VOTE&lt;lf&gt;id=x a vote is made by all higher nodes
        /// </summary>
        public const  string VOTE = "VOTE";

        public static OperationalMessage? MessageFrom(string content)
        {
            if (content.StartsWith(APP))
            {
                return ApplicationSays.From(content);
            }

            if (content.StartsWith(DIR))
            {
                return Directory.From(content);
            }

            if (content.StartsWith(ELECT))
            {
                return Elect.From(content);
            }

            if (content.StartsWith(JOIN))
            {
                return Join.From(content);
            }

            if (content.StartsWith(LEADER))
            {
                return Leader.From(content);
            }

            if (content.StartsWith(LEAVE))
            {
                return Leave.From(content);
            }

            if (content.StartsWith(PING))
            {
                return Ping.From(content);
            }

            if (content.StartsWith(PULSE))
            {
                return Pulse.From(content);
            }

            if (content.StartsWith(SPLIT))
            {
                return Split.From(content);
            }

            if (content.StartsWith(VOTE))
            {
                return Vote.From(content);
            }
            
            return null;
        }
        
        public Id Id { get; }

        public virtual bool IsApp => false;

        public virtual bool IsDirectory => false;

        public virtual bool IsElect => false;

        public virtual bool IsCheckHealth => false;

        public virtual bool IsJoin => false;

        public virtual bool IsLeader => false;

        public virtual bool IsLeave => false;

        public virtual bool IsPing => false;

        public virtual bool IsPulse => false;

        public virtual bool IsSplit => false;

        public virtual bool IsVote => false;

        protected OperationalMessage(Id id) => Id = id;
    }
}