using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_INSERT_MAP_TRANSITION =
            "INSERT INTO `nec_map_transition` (`map_id`, `transition_map_id`, `x`,  `y`, `z`, `maplink_heading`, `maplink_color`, `maplink_offset`, `maplink_width`, `distance`, `left_x`, `left_y`, `left_z`, `right_x`, `right_y`, `right_z`, `invertedPos`, `to_x`, `to_y`, `to_z`, `to_heading`, `state`, `created`, `updated`) VALUES (@map_id,@transition_map_id, @x, @y, @z, @maplink_heading,@maplink_color,@maplink_offset,@maplink_width,@distance, @left_x, @left_y, @left_z, @right_x, @right_y, @right_z, @invertedPos, @to_x, @to_y, @to_z, @to_heading, @state, @created, @updated);";

        private const string SQL_SELECT_MAP_TRANSITIONS =
            "SELECT `id`, `transition_map_id`, `map_id`, `x`,  `y`, `z`, `maplink_heading`, `maplink_color`, `maplink_offset`, `maplink_width`, `distance`, `left_x`, `left_y`, `left_z`, `right_x`, `right_y`, `right_z`, `invertedPos`, `to_x`, `to_y`, `to_z`, `to_heading`, `state`, `created`, `updated` FROM `nec_map_transition`;";

        private const string SQL_SELECT_MAP_TRANSITIONS_BY_ID =
            "SELECT `id`, `transition_map_id`, `map_id`, `x`,  `y`, `z`, `maplink_heading`, `maplink_color`, `maplink_offset`, `maplink_width`, `distance`, `left_x`, `left_y`, `left_z`, `right_x`, `right_y`, `right_z`, `invertedPos`, `to_x`, `to_y`, `to_z`, `to_heading`, `state`, `created`, `updated` FROM `nec_map_transition` WHERE `id`=@id;";

        private const string SQL_SELECT_MAP_TRANSITIONS_BY_MAP_ID =
            "SELECT `id`, `transition_map_id`, `map_id`, `x`,  `y`, `z`, `maplink_heading`, `maplink_color`, `maplink_offset`, `maplink_width`, `distance`, `left_x`, `left_y`, `left_z`, `right_x`, `right_y`, `right_z`, `invertedPos`, `to_x`, `to_y`, `to_z`, `to_heading`, `state`, `created`, `updated` FROM `nec_map_transition` WHERE `map_id`=@map_id;";

        private const string SQL_UPDATE_MAP_TRANSITION =
            "UPDATE `nec_map_transition` SET `id`=@id, `map_id`=@map_id, `transition_map_id`=@transition_map_id, `x`=@x,  `y`=@y, `z`=@z, `maplink_heading`=@maplink_heading, `maplink_color`=@maplink_color, `maplink_offset`=@maplink_offset, `maplink_width`=@maplink_width, `distance`=@distance, `left_x`=@left_x,  `left_y`=@left_y, `left_z`=@left_z, `right_x`=@right_x, `right_y`=@right_y, `right_z`=@right_z, `invertedPos`=@invertedPos, `to_x`=@to_x, `to_y`=@to_y, `to_z`=@to_z,`to_heading`=@to_heading,`state`=@state,`created`=@created,`updated`=@updated WHERE `id`=@id;";

        private const string SQL_DELETE_MAP_TRANSITION =
            "DELETE FROM `nec_map_transition` WHERE `id`=@id;";

        public bool InsertMapTransition(MapTransition mapTran)
        {
            int rowsAffected = ExecuteNonQuery(SQL_INSERT_MAP_TRANSITION, command =>
            {
                //AddParameter(command, "@id", gimmick.Id);
                AddParameter(command, "@map_id", mapTran.MapId);
                AddParameter(command, "@transition_map_id", mapTran.TransitionMapId);
                AddParameter(command, "@x", mapTran.ReferencePos.X);
                AddParameter(command, "@y", mapTran.ReferencePos.Y);
                AddParameter(command, "@z", mapTran.ReferencePos.Z);
                AddParameter(command, "@maplink_heading", mapTran.MaplinkHeading);
                AddParameter(command, "@maplink_color", mapTran.MaplinkColor);
                AddParameter(command, "@maplink_offset", mapTran.MaplinkOffset);
                AddParameter(command, "@maplink_width", mapTran.MaplinkWidth);
                AddParameter(command, "@distance", mapTran.RefDistance);
                AddParameter(command, "@left_x", mapTran.LeftPos.X);
                AddParameter(command, "@left_y", mapTran.LeftPos.Y);
                AddParameter(command, "@left_z", mapTran.LeftPos.Z);
                AddParameter(command, "@right_x", mapTran.RightPos.X);
                AddParameter(command, "@right_y", mapTran.RightPos.Y);
                AddParameter(command, "@right_z", mapTran.RightPos.Z);
                AddParameter(command, "@invertedPos", mapTran.InvertedTransition);
                AddParameter(command, "@to_x", mapTran.ToPos.x);
                AddParameter(command, "@to_y", mapTran.ToPos.y);
                AddParameter(command, "@to_z", mapTran.ToPos.z);
                AddParameter(command, "@to_heading", mapTran.ToPos.heading);
                AddParameter(command, "@state", mapTran.state);
                AddParameter(command, "@created", mapTran.created);
                AddParameter(command, "@updated", mapTran.updated);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED || autoIncrement <= NO_AUTO_INCREMENT) return false;

            mapTran.id = (int)autoIncrement;
            return true;
        }

        public List<MapTransition> SelectMapTransitions()
        {
            List<MapTransition> mapTrans = new List<MapTransition>();
            ExecuteReader(SQL_SELECT_MAP_TRANSITIONS, reader =>
            {
                while (reader.Read())
                {
                    MapTransition mapTran = ReadMapTransition(reader);
                    mapTrans.Add(mapTran);
                }
            });
            return mapTrans;
        }

        public MapTransition SelectMapTransitionsById(int id)
        {
            MapTransition mapTrans = null;
            ExecuteReader(SQL_SELECT_MAP_TRANSITIONS_BY_ID,
                command => { AddParameter(command, "@id", id); },
                reader =>
                {
                    if (reader.Read()) mapTrans = ReadMapTransition(reader);
                });
            return mapTrans;
        }

        public List<MapTransition> SelectMapTransitionsByMapId(int mapId)
        {
            List<MapTransition> mapTrans = new List<MapTransition>();
            ExecuteReader(SQL_SELECT_MAP_TRANSITIONS_BY_MAP_ID,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        MapTransition mapTran = ReadMapTransition(reader);
                        mapTrans.Add(mapTran);
                    }
                });
            return mapTrans;
        }


        public bool UpdateMapTransition(MapTransition mapTran)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_MAP_TRANSITION, command =>
            {
                AddParameter(command, "@id", mapTran.id);
                AddParameter(command, "@map_id", mapTran.MapId);
                AddParameter(command, "@transition_map_id", mapTran.TransitionMapId);
                AddParameter(command, "@x", mapTran.ReferencePos.X);
                AddParameter(command, "@y", mapTran.ReferencePos.Y);
                AddParameter(command, "@z", mapTran.ReferencePos.Z);
                AddParameter(command, "@maplink_heading", mapTran.MaplinkHeading);
                AddParameter(command, "@maplink_color", mapTran.MaplinkColor);
                AddParameter(command, "@maplink_offset", mapTran.MaplinkOffset);
                AddParameter(command, "@maplink_width", mapTran.MaplinkWidth);
                AddParameter(command, "@distance", mapTran.RefDistance);
                AddParameter(command, "@left_x", mapTran.LeftPos.X);
                AddParameter(command, "@left_y", mapTran.LeftPos.Y);
                AddParameter(command, "@left_z", mapTran.LeftPos.Z);
                AddParameter(command, "@right_x", mapTran.RightPos.X);
                AddParameter(command, "@right_y", mapTran.RightPos.Y);
                AddParameter(command, "@right_z", mapTran.RightPos.Z);
                AddParameter(command, "@invertedPos", mapTran.InvertedTransition);
                AddParameter(command, "@to_x", mapTran.ToPos.x);
                AddParameter(command, "@to_y", mapTran.ToPos.y);
                AddParameter(command, "@to_z", mapTran.ToPos.z);
                AddParameter(command, "@to_heading", mapTran.ToPos.heading);
                AddParameter(command, "@state", mapTran.state);
                AddParameter(command, "@created", mapTran.created);
                AddParameter(command, "@updated", mapTran.updated);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteMapTransition(int mapTranId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_MAP_TRANSITION,
                command => { AddParameter(command, "@id", mapTranId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        private MapTransition ReadMapTransition(DbDataReader reader)
        {
            MapTransition mapTran = new MapTransition();
            mapTran.id = GetInt32(reader, "id");
            mapTran.MapId = GetInt32(reader, "map_id");
            mapTran.TransitionMapId = GetInt32(reader, "transition_map_id");
            mapTran.ReferencePos.X = GetFloat(reader, "x");
            mapTran.ReferencePos.Y = GetFloat(reader, "y");
            mapTran.ReferencePos.Z = GetFloat(reader, "z");
            mapTran.MaplinkHeading = GetByte(reader, "maplink_heading");
            mapTran.MaplinkColor = GetInt32(reader, "maplink_color");
            mapTran.MaplinkOffset = GetInt32(reader, "maplink_offset");
            mapTran.MaplinkWidth = GetInt32(reader, "maplink_width");
            mapTran.RefDistance = GetInt32(reader, "distance");
            mapTran.LeftPos.X = GetFloat(reader, "left_x");
            mapTran.LeftPos.Y = GetFloat(reader, "left_y");
            mapTran.LeftPos.Z = GetFloat(reader, "left_z");
            mapTran.RightPos.X = GetFloat(reader, "right_x");
            mapTran.RightPos.Y = GetFloat(reader, "right_y");
            mapTran.RightPos.Z = GetFloat(reader, "right_z");
            mapTran.InvertedTransition = GetBoolean(reader, "invertedPos");
            mapTran.ToPos.x = GetFloat(reader, "to_x");
            mapTran.ToPos.y = GetFloat(reader, "to_y");
            mapTran.ToPos.z = GetFloat(reader, "to_z");
            mapTran.ToPos.heading = GetByte(reader, "to_heading");
            mapTran.state = GetInt32(reader, "state") == 0 ? false : true;
            mapTran.created = GetDateTime(reader, "created");
            mapTran.updated = GetDateTime(reader, "updated");
            return mapTran;
        }
    }
}
