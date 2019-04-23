// Copyright © 2012-2018 Vaughn Vernon. All rights reserved.
//
// This Source Code Form is subject to the terms of the
// Mozilla Public License, v. 2.0. If a copy of the MPL
// was not distributed with this file, You can obtain
// one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Vlingo.Cluster.Model
{
    public sealed class Properties
    {
        private static readonly string _propertiesFile = "/vlingo-cluster.properties";

        private static Func<Properties> _factory = () => Open();

        private static Lazy<Properties> SingleInstance => new Lazy<Properties>(_factory, true);
        
        private readonly IDictionary<string, string> _dictionary;

        public static Properties Instance => SingleInstance.Value;
        
        public static Properties OpenForTest(Dictionary<string, string> properties) => new Properties(properties);

        public static Properties Open()
        {
            var props = new Properties(new Dictionary<string, string>());
            props.Load(new FileInfo(_propertiesFile));
            return props;
        }
        
        public int ApplicationBufferSize() => GetInteger("cluster.app.buffer.size", 10240);

        public long ApplicationInboundProbeInterval()
        {
            var probeInterval = GetInteger("cluster.app.incoming.probe.interval", 100);
            
            if (probeInterval == 0) {
                throw new InvalidOperationException("Must assign an application (app) incoming probe interval in properties file.");
            }

            return probeInterval;
        }
        
        public int ApplicationOutgoingPooledBuffers()
        {
            var pooledBuffers = GetInteger("cluster.app.outgoing.pooled.buffers", 50);
            
            if (pooledBuffers == 0) {
                throw new InvalidOperationException("Must assign an application (app) pooled buffers size in properties file.");
            }

            return pooledBuffers;
        }

        public int ApplicationPort(string nodeName)
        {
            var port = GetInteger(nodeName, "app.port", 0);
            
            if (port == 0) {
                throw new InvalidOperationException($"Must assign an application (app) port to node '{nodeName}' in properties file.");
            }

            return port;
        }

        public Type ClusterApplicationType()
        {
            var typeName = ClusterApplicationTypeName();

            try
            {
                return Type.GetType(typeName);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Must define property: cluster.app.class", e);
            }
        }

        public string ClusterApplicationTypeName()
        {
            var typeName = GetString("cluster.app.class", "");

            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new InvalidOperationException("Must assign a cluster app class in properties file.");
            }

            return typeName;
        }

        public string ClusterApplicationStageName()
        {
            var name = GetString("cluster.app.stage", "");

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException("Must assign a cluster app stage name in properties file.");
            }

            return name;
        }
        
        public long ClusterAttributesRedistributionInterval()
        {
            var interval = GetInteger("cluster.attributes.redistribution.interval", 1000);
            return interval;
        }
  
        public int ClusterAttributesRedistributionRetries()
        {
            var interval = GetInteger("cluster.attributes.redistribution.retries", 10);
            return interval;
        }
  
        public long ClusterHealthCheckInterval()
        {
            var interval = GetInteger("cluster.health.check.interval", 3000);
            return interval;
        }
  
        public long ClusterHeartbeatInterval()
        {
            var interval = GetInteger("cluster.heartbeat.interval", 7000);
            return interval;
        }

        public long ClusterLiveNodeTimeout()
        {
            var timeout = GetInteger("cluster.live.node.timeout", 20000);
            return timeout;
        }

        public long ClusterQuorumTimeout()
        {
            var timeout = GetInteger("cluster.quorum.timeout", 60000);
            return timeout;
        }
        
        public string Host(string nodeName)
        {
            var host = GetString(nodeName, "host", "");

            if (string.IsNullOrWhiteSpace(host))
            {
                throw new InvalidOperationException($"Must assign a host to node '{nodeName}' in properties file.");
            }

            return host;
        }

        public short NodeId(string nodeName)
        {
            var nodeId = GetInteger(nodeName, "id", -1);

            if (nodeId == -1)
            {
                throw new InvalidOperationException($"Must assign an id to node '{nodeName}' in properties file.");
            }

            return (short) nodeId;
        }

        public string NodeName(string nodeName)
        {
            var name = GetString(nodeName, "name", "");

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidOperationException($"Must assign a name to node '{nodeName}' in properties file.");
            }

            return name;
        }

        public int OperationalBufferSize()
        {
            var size = GetInteger("cluster.op.buffer.size", 4096);
            return size;
        }

        public long OperationalInboundProbeInterval()
        {
            var probeInterval = GetInteger("cluster.op.incoming.probe.interval", 100);

            if (probeInterval == 0)
            {
                throw new InvalidOperationException("Must assign an operational (op) incoming probe interval in properties file.");
            }

            return probeInterval;
        }
        
        public int OperationalOutgoingPooledBuffers()
        {
            var pooledBuffers = GetInteger("cluster.op.outgoing.pooled.buffers", 20);

            if (pooledBuffers == 0)
            {
                throw new InvalidOperationException("Must assign an operational (op) pooled buffers size in properties file.");
            }

            return pooledBuffers;
        }
  
        public int OperationalPort(String nodeName)
        {
            var port = GetInteger(nodeName, "op.port", 0);

            if (port == 0)
            {
                throw new InvalidOperationException($"Must assign an operational (op) port to node '{nodeName}' in properties file.");
            }

            return port;
        }

        public IEnumerable<string> SeedNodes()
        {
            var seedNodes = new List<string>();

            var commaSeparated = GetString("cluster.seedNodes", "");

            if (string.IsNullOrWhiteSpace(commaSeparated))
            {
                throw new InvalidOperationException("Must declare seed nodes in properties file.");
            }

            foreach (var seedNode in commaSeparated.Split(','))
            {
                seedNodes.Add(seedNode);
            }

            return seedNodes;
        }

        public bool UseSSL() => GetBoolean("cluster.ssl", false);
        
        public bool GetBoolean(string nodeName, string key, bool defaultValue)
        {
            return bool.Parse(GetString(nodeName, key, defaultValue.ToString()));
        }

        public bool GetBoolean(string key, bool defaultValue)
        {
            return GetBoolean("", key, defaultValue);
        }
        
        public float GetFloat(string nodeName, string key, float defaultValue)
        {
            return float.Parse(GetString(nodeName, key, defaultValue.ToString(CultureInfo.InvariantCulture)));
        }

        public float GetFloat(string key, float defaultValue)
        {
            return GetFloat("", key, defaultValue);
        }
        
        public int GetInteger(string nodeName, string key, int defaultValue)
        {
            return int.Parse(GetString(nodeName, key, defaultValue.ToString()));
        }

        public int GetInteger(string key, int defaultValue)
        {
            return GetInteger("", key, defaultValue);
        }
        
        public string GetString(string nodeName, string key, string defaultValue)
        {
            return GetProperty(Key(nodeName, key), defaultValue);
        }

        public string GetString(string key, string defaultValue)
        {
            return GetProperty(key, defaultValue);
        }

        public void ValidateRequired(string nodeName)
        {
            // assertions in each accessor

            NodeName(nodeName);

            NodeId(nodeName);

            Host(nodeName);

            OperationalPort(nodeName);

            ApplicationPort(nodeName);

            SeedNodes();

            ClusterApplicationTypeName();
        }

        private Properties(Dictionary<string, string> properties)
        {
            _dictionary = properties;
        }
        
        private string GetProperty(string key) => GetProperty(key, null);

        private string GetProperty(string key, string defaultValue)
        {
            if(_dictionary.TryGetValue(key, out string value))
            {
                return value;
            }

            return defaultValue;
        }
        
        private void SetProperty(string key, string value)
        {
            _dictionary[key] = value;
        }
        
        private void Load(FileInfo configFile)
        {
            foreach(var line in File.ReadAllLines(configFile.FullName))
            {
                if(string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                {
                    continue;
                }

                var items = line.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                var key = items[0].Trim();
                var val = string.Join("=", items.Skip(1)).Trim();
                SetProperty(key, val);
            }
        }

        private string Key(string nodeName, string key)
        {
            if (string.IsNullOrWhiteSpace(nodeName))
            {
                return key;
            }

            return $"node.{nodeName}.{key}";
        }
    }
}