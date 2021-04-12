using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string _SqlInsertGimmick =
            "INSERT INTO `nec_gimmick_spawn` (`map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `state`, `created`, `updated`) VALUES (@map_id, @x, @y, @z, @heading, @model_id, @state, @created, @updated);";

        private const string _SqlSelectGimmicks =
            "SELECT `id`, `map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `state`, `created`, `updated` FROM `nec_gimmick_spawn`;";

        private const string _SqlSelectGimmicksByMapId =
            "SELECT `id`, `map_id`, `x`,  `y`, `z`, `heading`, `model_id`, `state`, `created`, `updated` FROM `nec_gimmick_spawn` WHERE `map_id`=@map_id;";

        private const string _SqlUpdateGimmick =
            "UPDATE `nec_gimmick_spawn` SET `id`=@id, `map_id`=@map_id, `x`=@x,  `y`=@y, `z`=@z, `heading`=@heading, `model_id`=@model_id, `state`=@state, `created`=@created, `updated`=@updated WHERE `id`=@id;";

        private const string _SqlDeleteGimmick =
            "DELETE FROM `nec_gimmick_spawn` WHERE `id`=@id;";

        public bool InsertGimmick(Gimmick gimmick)
        {
            int rowsAffected = ExecuteNonQuery(_SqlInsertGimmick, command =>
            {
                //AddParameter(command, "@id", gimmick.Id);
                AddParameter(command, "@map_id", gimmick.mapId);
                AddParameter(command, "@x", gimmick.x);
                AddParameter(command, "@y", gimmick.y);
                AddParameter(command, "@z", gimmick.z);
                AddParameter(command, "@heading", gimmick.heading);
                AddParameter(command, "@model_id", gimmick.modelId);
                AddParameter(command, "@state", gimmick.state);
                AddParameter(command, "@created", gimmick.created);
                AddParameter(command, "@updated", gimmick.updated);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement) return false;

            gimmick.id = (int)autoIncrement;
            return true;
        }

        public List<Gimmick> SelectGimmicks()
        {
            List<Gimmick> gimmicks = new List<Gimmick>();
            ExecuteReader(_SqlSelectGimmicks, reader =>
            {
                while (reader.Read())
                {
                    Gimmick gimmick = ReadGimmick(reader);
                    gimmicks.Add(gimmick);
                }
            });
            return gimmicks;
        }

        public List<Gimmick> SelectGimmicksByMapId(int mapId)
        {
            List<Gimmick> gimmicks = new List<Gimmick>();
            ExecuteReader(_SqlSelectGimmicksByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        Gimmick gimmick = ReadGimmick(reader);
                        gimmicks.Add(gimmick);
                    }
                });
            return gimmicks;
        }

        public bool UpdateGimmick(Gimmick gimmick)
        {
            int rowsAffected = ExecuteNonQuery(_SqlUpdateGimmick, command =>
            {
                AddParameter(command, "@id", gimmick.id);
                AddParameter(command, "@map_id", gimmick.mapId);
                AddParameter(command, "@x", gimmick.x);
                AddParameter(command, "@y", gimmick.y);
                AddParameter(command, "@z", gimmick.z);
                AddParameter(command, "@heading", gimmick.heading);
                AddParameter(command, "@model_id", gimmick.modelId);
                AddParameter(command, "@state", gimmick.state);
                AddParameter(command, "@created", gimmick.created);
                AddParameter(command, "@updated", gimmick.updated);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteGimmick(int gimmickId)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteGimmick,
                command => { AddParameter(command, "@id", gimmickId); });
            return rowsAffected > NoRowsAffected;
        }

        private Gimmick ReadGimmick(DbDataReader reader)
        {
            Gimmick gimmick = new Gimmick();
            gimmick.id = GetInt32(reader, "id");
            gimmick.mapId = GetInt32(reader, "map_id");
            gimmick.x = GetFloat(reader, "x");
            gimmick.y = GetFloat(reader, "y");
            gimmick.z = GetFloat(reader, "z");
            gimmick.heading = (byte)GetInt32(reader, "heading");
            gimmick.modelId = GetInt32(reader, "model_id");
            gimmick.state = GetInt32(reader, "state");
            gimmick.created = GetDateTime(reader, "created");
            gimmick.updated = GetDateTime(reader, "updated");
            return gimmick;
        }
    }
}
