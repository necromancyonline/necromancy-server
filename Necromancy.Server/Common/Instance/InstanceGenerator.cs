using System.Collections.Generic;
using System.Text;
using Arrowgene.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Common.Instance
{
    /// <summary>
    /// Provides Unique Ids for instancing.
    /// </summary>
    public class InstanceGenerator
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(InstanceGenerator));

        public const uint InvalidInstanceId = 0;

        private readonly Dictionary<uint, IInstance> _instances;
        private readonly InstanceIdPool _dynamicPool;
        private readonly DatabaseInstanceIdPool _characterPool;
        private readonly DatabaseInstanceIdPool _npcPool;
        private readonly DatabaseInstanceIdPool _monsterPool;
        private readonly DatabaseInstanceIdPool _deadBodyPool;
        private readonly List<IInstanceIdPool> _pools;
        private readonly NecServer _server;
        private readonly NecSetting _setting;

        public InstanceGenerator(NecServer server)
        {
            _setting = server.Setting;
            _server = server;
            _pools = new List<IInstanceIdPool>();
            _instances = new Dictionary<uint, IInstance>();
            _dynamicPool = new InstanceIdPool("Dynamic", _setting.PoolDynamicIdLowerBound, _setting.PoolDynamicIdSize);
            _pools.Add(_dynamicPool);
            _characterPool = new DatabaseInstanceIdPool("Character", _setting.PoolCharacterIdLowerBound,   _setting.PoolCharacterIdSize);
            _pools.Add(_characterPool);
            _npcPool = new DatabaseInstanceIdPool("Npc", _setting.PoolNpcLowerBound, _setting.PoolNpcIdSize);
            _pools.Add(_npcPool);
            _monsterPool = new DatabaseInstanceIdPool("Monster", _setting.PoolMonsterIdLowerBound, _setting.PoolMonsterIdSize);
            _pools.Add(_monsterPool);
            _deadBodyPool = new DatabaseInstanceIdPool("DeadBody", _setting.PoolDeadBodyIdLowerBound, _setting.PoolDeadBodyIdSize);
            _pools.Add(_deadBodyPool);

            foreach (IInstanceIdPool pool in _pools)
            {
                foreach (IInstanceIdPool otherPool in _pools)
                {
                    if (pool == otherPool)
                    {
                        continue;
                    }

                    if (pool.LowerBound <= otherPool.UpperBound && otherPool.LowerBound <= pool.UpperBound)
                    {
                        Logger.Error(
                            $"Pool: {pool.Name}({pool.LowerBound}-{pool.UpperBound}) overlaps with Pool {otherPool.Name}({otherPool.LowerBound}-{otherPool.UpperBound})");
                    }
                }
            }

            LogStatus();
        }

        public Character GetCharacterByDatabaseId(int characterDatabaseId)
        {
            uint characterInstanceId = _characterPool.GetInstanceId((uint) characterDatabaseId);
            IInstance instance = GetInstance(characterInstanceId);
            if (instance is Character character)
            {
                return character;
            }

            character = _server.Clients.GetCharacterByCharacterId(characterDatabaseId);
            if (character != null)
            {
                Logger.Error(
                    $"Character {character.Name} in server lookup but not in instance lookup - fix synchronisation");
                return character;
            }

            character = _server.Database.SelectCharacterById(characterDatabaseId);
            if (character != null)
            {
                character.InstanceId = characterInstanceId;
                return character;
            }

            return null;
        }

        public Character GetCharacterByInstanceId(uint characterInstanceId)
        {
            int characterDatabaseId = _characterPool.GetDatabaseId(characterInstanceId);
            return GetCharacterByDatabaseId(characterDatabaseId);
        }

        public uint GetCharacterInstanceId(int characterDatabaseId)
        {
            return _characterPool.GetInstanceId((uint) characterDatabaseId);
        }

        public int GetCharacterDatabaseId(uint characterInstanceId)
        {
            return _characterPool.GetDatabaseId(characterInstanceId);
        }

        /// <summary>
        /// Creates a lookup for the instance and assigns an InstanceId.
        /// </summary>
        public void AssignInstance(IInstance instance)
        {
            uint instanceId;
            bool success;
            if (instance is Character character)
            {
                success = _characterPool.TryAssign((uint)character.Id, out instanceId);
            }
            else if (instance is DeadBody deadBody)
            {
                success = _deadBodyPool.TryAssign((uint)deadBody.Id, out instanceId);
            }
            else if (instance is NpcSpawn npc)
            {
                success = _npcPool.TryAssign((uint) npc.Id, out instanceId);
            }
            else if (instance is MonsterSpawn monster)
            {
                success = _monsterPool.TryAssign((uint) monster.Id, out instanceId);
            }
            else if (_dynamicPool.TryPop(out instanceId))
            {
                success = true;
            }
            else
            {
                instanceId = InvalidInstanceId;
                success = false;
            }

            if (instanceId == InvalidInstanceId)
            {
                Logger.Error($"Failed to retrieve instanceId for type {instance.GetType()}");
                return;
            }

            if (!success)
            {
                if (_instances.ContainsKey(instanceId))
                {
                    // object already exists in lookup.
                    instance.InstanceId = instanceId;
                    return;
                }

                Logger.Error($"Failed to assign instanceId for type {instance.GetType()}");
                return;
            }

            instance.InstanceId = instanceId;
            _instances.Add(instanceId, instance);
        }

        /// <summary>
        /// Deletes the lookup.
        /// </summary>
        public void FreeInstance(IInstance instance)
        {
            uint instanceId = instance.InstanceId;
            if (instanceId == InvalidInstanceId)
            {
                Logger.Error("Failed to free, instanceId is invalid");
                return;
            }

            if (_instances.ContainsKey(instanceId))
            {
                _instances.Remove(instanceId);
            }

            if (instance is Character character)
            {
                _characterPool.Free(instanceId);
            }
            else if (instance is NpcSpawn npc)
            {
                _npcPool.Free(instanceId);
            }
            else if (instance is MonsterSpawn monster)
            {
                _monsterPool.Free(instanceId);
            }
            else
            {
                _dynamicPool.Push(instanceId);
            }
        }

        /// <summary>
        /// Retrieves an Instance by InstanceId
        /// </summary>
        public IInstance GetInstance(uint instanceId)
        {
            if (!_instances.ContainsKey(instanceId))
            {
                return null;
            }

            return _instances[instanceId];
        }

        public void LogStatus()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("");
            sb.AppendLine("--- IdPool Status ---");
            foreach (IInstanceIdPool pool in _pools)
            {
                sb.AppendLine(
                    $"{pool.Name}: {pool.Used}/{pool.Size} ({pool.LowerBound}-{pool.UpperBound})");
            }

            sb.AppendLine("---");
            Logger.Info(sb.ToString());
        }
    }
}
