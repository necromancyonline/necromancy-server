using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string _SqlInsertNpcSpawn =
            "INSERT INTO `nec_npc_spawn` (`npc_id`, `model_id`, `level`,  `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `visibility`, `created`, `updated`, `icon`, `status`, `status_x`, `status_y`, `status_z`) VALUES (@npc_id, @model_id, @level, @name, @title, @map_id, @x, @y, @z, @active, @heading, @size, @visibility, @created, @updated, @icon, @status, @status_x, @status_y, @status_z );";

        private const string _SqlSelectNpcSpawns =
            "SELECT `id`, `npc_id`, `model_id`, `level`, `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `visibility`, `created`, `updated` , `icon`, `status`, `status_x`, `status_y`, `status_z` FROM `nec_npc_spawn`;";

        private const string _SqlSelectNpcSpawnsByMapId =
            "SELECT `id`, `npc_id`, `model_id`, `level`, `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `visibility`, `created`, `updated` , `icon`, `status`, `status_x`, `status_y`, `status_z` FROM `nec_npc_spawn` WHERE `map_id`=@map_id;";

        private const string _SqlUpdateNpcSpawn =
            "UPDATE `nec_npc_spawn` SET `npc_id`=@npc_id, `model_id`=@model_id, `level`=@level,  `name`=@name, `title`=@title, `map_id`=@map_id, `x`=@x, `y`=@y, `z`=@z, `active`=@active, `heading`=@heading, `size`=@size, `visibility`=@visibility, `created`=@created, `updated`=@updated, `icon`=@icon, `status`=@status, `status_x`=@status_x, `status_y`=@status_y, `status_z`=@status_z WHERE `id`=@id;";

        private const string _SqlDeleteNpcSpawn =
            "DELETE FROM `nec_npc_spawn` WHERE `id`=@id;";

        public bool InsertNpcSpawn(NpcSpawn npcSpawn)
        {
            int rowsAffected = ExecuteNonQuery(_SqlInsertNpcSpawn, command =>
            {
                AddParameter(command, "@npc_id", npcSpawn.npcId);
                AddParameter(command, "@model_id", npcSpawn.modelId);
                AddParameter(command, "@level", npcSpawn.level);
                AddParameter(command, "@name", npcSpawn.name);
                AddParameter(command, "@title", npcSpawn.title);
                AddParameter(command, "@map_id", npcSpawn.mapId);
                AddParameter(command, "@x", npcSpawn.x);
                AddParameter(command, "@y", npcSpawn.y);
                AddParameter(command, "@z", npcSpawn.z);
                AddParameter(command, "@active", npcSpawn.active);
                AddParameter(command, "@heading", npcSpawn.heading);
                AddParameter(command, "@size", npcSpawn.size);
                AddParameter(command, "@visibility", npcSpawn.visibility);
                AddParameter(command, "@created", npcSpawn.created);
                AddParameter(command, "@updated", npcSpawn.updated);
                AddParameter(command, "@icon", npcSpawn.icon);
                AddParameter(command, "@status", npcSpawn.status);
                AddParameter(command, "@status_x", npcSpawn.statusX);
                AddParameter(command, "@status_y", npcSpawn.statusY);
                AddParameter(command, "@status_z", npcSpawn.statusZ);

            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            npcSpawn.id = (int)autoIncrement;
            return true;
        }

        public List<NpcSpawn> SelectNpcSpawns()
        {
            List<NpcSpawn> npcSpawns = new List<NpcSpawn>();
            ExecuteReader(_SqlSelectNpcSpawns, reader =>
            {
                while (reader.Read())
                {
                    NpcSpawn npcSpawn = ReadNpcSpawn(reader);
                    npcSpawns.Add(npcSpawn);
                }
            });
            return npcSpawns;
        }

        public List<NpcSpawn> SelectNpcSpawnsByMapId(int mapId)
        {
            List<NpcSpawn> npcSpawns = new List<NpcSpawn>();
            ExecuteReader(_SqlSelectNpcSpawnsByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        NpcSpawn npcSpawn = ReadNpcSpawn(reader);
                        npcSpawns.Add(npcSpawn);
                    }
                });
            return npcSpawns;
        }

        /*public List<DeadBody> SelectDeadBodiesByMapId(int mapId)
        {
            List<DeadBody> deadBodies = new List<DeadBody>();
            ExecuteReader(SqlSelectDeadBodyByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        DeadBody deadBodies = ReadDeadBody(reader);
                        deadBodies.Add(deadBodies);
                    }
                });
            return deadBodies;
        }*/

        public bool UpdateNpcSpawn(NpcSpawn npcSpawn)
        {
            int rowsAffected = ExecuteNonQuery(_SqlUpdateNpcSpawn, command =>
            {
                AddParameter(command, "@id", npcSpawn.id);
                AddParameter(command, "@npc_id", npcSpawn.npcId);
                AddParameter(command, "@model_id", npcSpawn.modelId);
                AddParameter(command, "@level", npcSpawn.level);
                AddParameter(command, "@name", npcSpawn.name);
                AddParameter(command, "@title", npcSpawn.title);
                AddParameter(command, "@map_id", npcSpawn.mapId);
                AddParameter(command, "@x", npcSpawn.x);
                AddParameter(command, "@y", npcSpawn.y);
                AddParameter(command, "@z", npcSpawn.z);
                AddParameter(command, "@active", npcSpawn.active);
                AddParameter(command, "@heading", npcSpawn.heading);
                AddParameter(command, "@size", npcSpawn.size);
                AddParameter(command, "@visibility", npcSpawn.visibility);
                AddParameter(command, "@created", npcSpawn.created);
                AddParameter(command, "@updated", npcSpawn.updated);
                AddParameter(command, "@icon", npcSpawn.icon);
                AddParameter(command, "@status", npcSpawn.status);
                AddParameter(command, "@status_x", npcSpawn.statusX);
                AddParameter(command, "@status_y", npcSpawn.statusY);
                AddParameter(command, "@status_z", npcSpawn.statusZ);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteNpcSpawn(int npcSpawnId)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteNpcSpawn,
                command => { AddParameter(command, "@id", npcSpawnId); });
            return rowsAffected > NoRowsAffected;
        }

        private NpcSpawn ReadNpcSpawn(DbDataReader reader)
        {
            NpcSpawn npcSpawn = new NpcSpawn();
            npcSpawn.id = GetInt32(reader, "id");
            npcSpawn.modelId = GetInt32(reader, "model_id");
            npcSpawn.npcId = GetInt32(reader, "npc_id");
            npcSpawn.level = GetByte(reader, "level");
            npcSpawn.name = GetString(reader, "name");
            npcSpawn.title = GetString(reader, "title");
            npcSpawn.mapId = GetInt32(reader, "map_id");
            npcSpawn.x = GetFloat(reader, "x");
            npcSpawn.y = GetFloat(reader, "y");
            npcSpawn.z = GetFloat(reader, "z");
            npcSpawn.active = GetBoolean(reader, "active");
            npcSpawn.heading = GetByte(reader, "heading");
            npcSpawn.size = GetInt16(reader, "size");
            npcSpawn.visibility = GetInt32(reader, "visibility");
            npcSpawn.created = GetDateTime(reader, "created");
            npcSpawn.updated = GetDateTime(reader, "updated");
            npcSpawn.icon = GetInt32(reader, "icon");
            npcSpawn.status = GetInt32(reader, "status");
            npcSpawn.statusX = GetInt32(reader, "status_x");
            npcSpawn.statusY = GetInt32(reader, "status_y");
            npcSpawn.statusZ = GetInt32(reader, "status_z");
            //Logger.Debug($"Reading Row {npcSpawn.Id}"); //for determining which row read throws SQL errors.
            return npcSpawn;
        }
    }
}
