using System.Collections.Concurrent;
using Arrowgene.Logging;
using Necromancy.Server.Database;

namespace Necromancy.Server.Common.Instance
{
    public class DatabaseInstanceIdPool : IInstanceIdPool
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(DatabaseInstanceIdPool));

        private readonly ConcurrentDictionary<uint, uint> _idPool;

        public DatabaseInstanceIdPool(string name, uint lowerBound, uint size)
        {
            _idPool = new ConcurrentDictionary<uint, uint>();
            this.name = name;
            this.lowerBound = lowerBound;
            this.size = size;
            upperBound = this.lowerBound + this.size;
        }

        public uint used => (uint)_idPool.Count;
        public uint lowerBound { get; }
        public uint upperBound { get; }
        public uint size { get; }
        public string name { get; }

        public uint GetInstanceId(uint dbId)
        {
            return dbId + lowerBound;
        }

        public int GetDatabaseId(uint instanceId)
        {
            if (instanceId < lowerBound || instanceId > upperBound)
            {
                _Logger.Error($"InstanceId: {instanceId} does not belong to pool {name} ({lowerBound}-{upperBound})");
                return IDatabase.InvalidDatabaseId;
            }

            return (int)(instanceId - lowerBound);
        }

        public bool TryAssign(uint dbId, out uint instanceId)
        {
            if (dbId > size)
            {
                _Logger.Error($"Exhausted pool {name} size of {size} for dbId: {dbId}");
                instanceId = InstanceGenerator.InvalidInstanceId;
                return false;
            }

            instanceId = GetInstanceId(dbId);
            if (_idPool.ContainsKey(instanceId))
                // Instance already recorded
                return false;

            if (!_idPool.TryAdd(instanceId, dbId))
            {
                _Logger.Error($"DbId: {dbId} already assigned to instanceId: {instanceId} for pool {name}");
                instanceId = InstanceGenerator.InvalidInstanceId;
                return false;
            }

            return true;
        }

        public bool Free(uint instanceId)
        {
            return _idPool.TryRemove(instanceId, out uint dbId);
        }
    }
}
