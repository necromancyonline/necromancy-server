using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string _SqlInsertGGateSpawn =
            "INSERT INTO `nec_gGate_spawn` (`serial_id`,`interaction`,`name`,`title`,`map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `size`,`active`,`glow`,`created`, `updated`) VALUES (@serial_id,@interaction,@name,@title,@map_id, @x, @y, @z, @heading, @model_id, @size, @active, @glow, @created, @updated);";

        private const string _SqlSelectGGateSpawns =
            "SELECT `id`, `serial_id`,`interaction`,`name`,`title`,`map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `size`,`active`,`glow`,`created`, `updated` FROM `nec_ggate_spawn`;";

        private const string _SqlSelectGGateSpawnsByMapId =
            "SELECT `id`, `serial_id`,`interaction`,`name`,`title`,`map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `size`,`active`,`glow`,`created`, `updated` FROM `nec_ggate_spawn` WHERE `map_id`=@map_id;";

        private const string _SqlUpdateGGateSpawn =
            "UPDATE `nec_ggate_spawn` SET `id`=@id,`serial_id`=@serial_id,`interaction`=@interaction,`name`=@name,`title`=@title,`map_id`=@map_id, `x`=@x,  `y`=@y, `z`=@z, `heading`=@heading, `model_id`=@model_id, `size`=@size,`active`=@active,`glow`=@glow,`created`=@created, `updated`=@updated WHERE `id`=@id;";

        private const string _SqlDeleteGGateSpawn =
            "DELETE FROM `nec_ggate_spawn` WHERE `id`=@id;";

        public bool InsertGGateSpawn(GGateSpawn gGateSpawn)
        {
            int rowsAffected = ExecuteNonQuery(_SqlInsertGGateSpawn, command =>
            {
                //AddParameter(command, "@id", gGateSpawn.Id);
                AddParameter(command, "@serial_id", gGateSpawn.serialId);
                AddParameter(command, "@interaction", gGateSpawn.interaction);
                AddParameter(command, "@name", gGateSpawn.name);
                AddParameter(command, "@title", gGateSpawn.title);
                AddParameter(command, "@map_id", gGateSpawn.mapId);
                AddParameter(command, "@x", gGateSpawn.x);
                AddParameter(command, "@y", gGateSpawn.y);
                AddParameter(command, "@z", gGateSpawn.z);
                AddParameter(command, "@heading", gGateSpawn.heading);
                AddParameter(command, "@model_id", gGateSpawn.modelId);
                AddParameter(command, "@size", gGateSpawn.size);
                AddParameter(command, "@active", gGateSpawn.active);
                AddParameter(command, "@glow", gGateSpawn.glow);
                AddParameter(command, "@created", gGateSpawn.created);
                AddParameter(command, "@updated", gGateSpawn.updated);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement) return false;

            gGateSpawn.id = (int)autoIncrement;
            return true;
        }

        public List<GGateSpawn> SelectGGateSpawns()
        {
            List<GGateSpawn> gGateSpawns = new List<GGateSpawn>();
            ExecuteReader(_SqlSelectGGateSpawns, reader =>
            {
                while (reader.Read())
                {
                    GGateSpawn gGateSpawn = ReadGGateSpawn(reader);
                    gGateSpawns.Add(gGateSpawn);
                }
            });
            return gGateSpawns;
        }

        public List<GGateSpawn> SelectGGateSpawnsByMapId(int mapId)
        {
            List<GGateSpawn> gGateSpawns = new List<GGateSpawn>();
            ExecuteReader(_SqlSelectGGateSpawnsByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        GGateSpawn gGateSpawn = ReadGGateSpawn(reader);
                        gGateSpawns.Add(gGateSpawn);
                    }
                });
            return gGateSpawns;
        }

        public bool UpdateGGateSpawn(GGateSpawn gGateSpawn)
        {
            int rowsAffected = ExecuteNonQuery(_SqlUpdateGGateSpawn, command =>
            {
                AddParameter(command, "@id", gGateSpawn.id);
                AddParameter(command, "@serial_id", gGateSpawn.serialId);
                AddParameter(command, "@interaction", gGateSpawn.interaction);
                AddParameter(command, "@name", gGateSpawn.name);
                AddParameter(command, "@title", gGateSpawn.title);
                AddParameter(command, "@map_id", gGateSpawn.mapId);
                AddParameter(command, "@x", gGateSpawn.x);
                AddParameter(command, "@y", gGateSpawn.y);
                AddParameter(command, "@z", gGateSpawn.z);
                AddParameter(command, "@heading", gGateSpawn.heading);
                AddParameter(command, "@model_id", gGateSpawn.modelId);
                AddParameter(command, "@size", gGateSpawn.size);
                AddParameter(command, "@active", gGateSpawn.active);
                AddParameter(command, "@glow", gGateSpawn.glow);
                AddParameter(command, "@created", gGateSpawn.created);
                AddParameter(command, "@updated", gGateSpawn.updated);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteGGateSpawn(int gGateSpawnId)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteGGateSpawn,
                command => { AddParameter(command, "@id", gGateSpawnId); });
            return rowsAffected > NoRowsAffected;
        }

        private GGateSpawn ReadGGateSpawn(DbDataReader reader)
        {
            GGateSpawn gGateSpawn = new GGateSpawn();
            gGateSpawn.id = GetInt32(reader, "id");
            gGateSpawn.serialId = GetInt32(reader, "serial_id");
            gGateSpawn.interaction = GetByte(reader, "interaction");
            gGateSpawn.name = GetString(reader, "name");
            gGateSpawn.title = GetString(reader, "title");
            gGateSpawn.mapId = GetInt32(reader, "map_id");
            gGateSpawn.x = GetFloat(reader, "x");
            gGateSpawn.y = GetFloat(reader, "y");
            gGateSpawn.z = GetFloat(reader, "z");
            gGateSpawn.heading = (byte)GetInt32(reader, "heading");
            gGateSpawn.modelId = GetInt32(reader, "model_id");
            gGateSpawn.size = GetInt16(reader, "size");
            gGateSpawn.active = GetInt32(reader, "active");
            gGateSpawn.glow = GetInt32(reader, "glow");
            gGateSpawn.created = GetDateTime(reader, "created");
            gGateSpawn.updated = GetDateTime(reader, "updated");
            return gGateSpawn;
        }
    }
}
