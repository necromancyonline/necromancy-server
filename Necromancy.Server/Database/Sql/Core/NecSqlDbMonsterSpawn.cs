using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string _SqlInsertMonsterSpawn =
            "INSERT INTO `nec_monster_spawn` (`monster_id`, `model_id`, `level`,  `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `created`, `updated`) VALUES (@monster_id, @model_id, @level, @name, @title, @map_id, @x, @y, @z, @active, @heading, @size, @created, @updated);";

        private const string _SqlSelectMonsterSpawns =
            "SELECT `id`, `monster_id`, `model_id`, `level`, `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `created`, `updated` FROM `nec_monster_spawn`;";

        private const string _SqlSelectMonsterSpawnsByMapId =
            "SELECT `id`, `monster_id`, `model_id`, `level`, `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `created`, `updated` FROM `nec_monster_spawn` WHERE `map_id`=@map_id;";

        private const string _SqlUpdateMonsterSpawn =
            "UPDATE `nec_monster_spawn` SET `monster_id`=@monster_id, `model_id`=@model_id, `level`=@level,  `name`=@name, `title`=@title, `map_id`=@map_id, `x`=@x, `y`=@y, `z`=@z, `active`=@active, `heading`=@heading, `size`=@size, `created`=@created, `updated`=@updated WHERE `id`=@id;";

        private const string _SqlDeleteMonsterSpawn =
            "DELETE FROM `nec_monster_spawn` WHERE `id`=@id;";

        public bool InsertMonsterSpawn(MonsterSpawn monsterSpawn)
        {
            int rowsAffected = ExecuteNonQuery(_SqlInsertMonsterSpawn, command =>
            {
                AddParameter(command, "@monster_id", monsterSpawn.monsterId);
                AddParameter(command, "@model_id", monsterSpawn.modelId);
                AddParameter(command, "@level", monsterSpawn.level);
                AddParameter(command, "@name", monsterSpawn.name);
                AddParameter(command, "@title", monsterSpawn.title);
                AddParameter(command, "@map_id", monsterSpawn.mapId);
                AddParameter(command, "@x", monsterSpawn.x);
                AddParameter(command, "@y", monsterSpawn.y);
                AddParameter(command, "@z", monsterSpawn.z);
                AddParameter(command, "@active", monsterSpawn.active);
                AddParameter(command, "@heading", monsterSpawn.heading);
                AddParameter(command, "@size", monsterSpawn.size);
                AddParameter(command, "@created", monsterSpawn.created);
                AddParameter(command, "@updated", monsterSpawn.updated);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement) return false;

            monsterSpawn.id = (int)autoIncrement;
            return true;
        }

        public List<MonsterSpawn> SelectMonsterSpawns()
        {
            List<MonsterSpawn> monsterSpawns = new List<MonsterSpawn>();
            ExecuteReader(_SqlSelectMonsterSpawns, reader =>
            {
                while (reader.Read())
                {
                    MonsterSpawn monsterSpawn = ReadMonsterSpawn(reader);
                    monsterSpawns.Add(monsterSpawn);
                }
            });
            return monsterSpawns;
        }

        public List<MonsterSpawn> SelectMonsterSpawnsByMapId(int mapId)
        {
            List<MonsterSpawn> monsterSpawns = new List<MonsterSpawn>();
            ExecuteReader(_SqlSelectMonsterSpawnsByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        MonsterSpawn monsterSpawn = ReadMonsterSpawn(reader);
                        monsterSpawns.Add(monsterSpawn);
                    }
                });
            return monsterSpawns;
        }

        public bool UpdateMonsterSpawn(MonsterSpawn monsterSpawn)
        {
            int rowsAffected = ExecuteNonQuery(_SqlUpdateMonsterSpawn, command =>
            {
                AddParameter(command, "@monster_id", monsterSpawn.monsterId);
                AddParameter(command, "@model_id", monsterSpawn.modelId);
                AddParameter(command, "@level", monsterSpawn.level);
                AddParameter(command, "@name", monsterSpawn.name);
                AddParameter(command, "@title", monsterSpawn.title);
                AddParameter(command, "@map_id", monsterSpawn.mapId);
                AddParameter(command, "@x", monsterSpawn.x);
                AddParameter(command, "@y", monsterSpawn.y);
                AddParameter(command, "@z", monsterSpawn.z);
                AddParameter(command, "@active", monsterSpawn.active);
                AddParameter(command, "@heading", monsterSpawn.heading);
                AddParameter(command, "@size", monsterSpawn.size);
                AddParameter(command, "@created", monsterSpawn.created);
                AddParameter(command, "@updated", monsterSpawn.updated);
                AddParameter(command, "@id", monsterSpawn.id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteMonsterSpawn(int monsterSpawnId)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteMonsterSpawn,
                command => { AddParameter(command, "@id", monsterSpawnId); });
            return rowsAffected > NoRowsAffected;
        }

        private MonsterSpawn ReadMonsterSpawn(DbDataReader reader)
        {
            MonsterSpawn monsterSpawn = new MonsterSpawn();
            monsterSpawn.id = GetInt32(reader, "id");
            monsterSpawn.modelId = GetInt32(reader, "model_id");
            monsterSpawn.monsterId = GetInt32(reader, "monster_id");
            monsterSpawn.level = GetByte(reader, "level");
            monsterSpawn.name = GetString(reader, "name");
            monsterSpawn.title = GetString(reader, "title");
            monsterSpawn.mapId = GetInt32(reader, "map_id");
            monsterSpawn.x = GetFloat(reader, "x");
            monsterSpawn.y = GetFloat(reader, "y");
            monsterSpawn.z = GetFloat(reader, "z");
            monsterSpawn.active = GetBoolean(reader, "active");
            monsterSpawn.heading = GetByte(reader, "heading");
            monsterSpawn.size = GetInt16(reader, "size");
            monsterSpawn.created = GetDateTime(reader, "created");
            monsterSpawn.updated = GetDateTime(reader, "updated");
            return monsterSpawn;
        }
    }
}
