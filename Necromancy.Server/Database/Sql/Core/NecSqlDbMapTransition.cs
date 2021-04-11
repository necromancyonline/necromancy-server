using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string _SqlInsertMapTransition =
            "INSERT INTO `nec_map_transition` (`map_id`, `transition_map_id`, `x`,  `y`, `z`, `maplink_heading`, `maplink_color`, `maplink_offset`, `maplink_width`, `distance`, `left_x`, `left_y`, `left_z`, `right_x`, `right_y`, `right_z`, `invertedPos`, `to_x`, `to_y`, `to_z`, `to_heading`, `state`, `created`, `updated`) VALUES (@map_id,@transition_map_id, @x, @y, @z, @maplink_heading,@maplink_color,@maplink_offset,@maplink_width,@distance, @left_x, @left_y, @left_z, @right_x, @right_y, @right_z, @invertedPos, @to_x, @to_y, @to_z, @to_heading, @state, @created, @updated);";

        private const string _SqlSelectMapTransitions =
            "SELECT `id`, `transition_map_id`, `map_id`, `x`,  `y`, `z`, `maplink_heading`, `maplink_color`, `maplink_offset`, `maplink_width`, `distance`, `left_x`, `left_y`, `left_z`, `right_x`, `right_y`, `right_z`, `invertedPos`, `to_x`, `to_y`, `to_z`, `to_heading`, `state`, `created`, `updated` FROM `nec_map_transition`;";

        private const string _SqlSelectMapTransitionsById =
            "SELECT `id`, `transition_map_id`, `map_id`, `x`,  `y`, `z`, `maplink_heading`, `maplink_color`, `maplink_offset`, `maplink_width`, `distance`, `left_x`, `left_y`, `left_z`, `right_x`, `right_y`, `right_z`, `invertedPos`, `to_x`, `to_y`, `to_z`, `to_heading`, `state`, `created`, `updated` FROM `nec_map_transition` WHERE `id`=@id;";

        private const string _SqlSelectMapTransitionsByMapId =
            "SELECT `id`, `transition_map_id`, `map_id`, `x`,  `y`, `z`, `maplink_heading`, `maplink_color`, `maplink_offset`, `maplink_width`, `distance`, `left_x`, `left_y`, `left_z`, `right_x`, `right_y`, `right_z`, `invertedPos`, `to_x`, `to_y`, `to_z`, `to_heading`, `state`, `created`, `updated` FROM `nec_map_transition` WHERE `map_id`=@map_id;";

        private const string _SqlUpdateMapTransition =
            "UPDATE `nec_map_transition` SET `id`=@id, `map_id`=@map_id, `transition_map_id`=@transition_map_id, `x`=@x,  `y`=@y, `z`=@z, `maplink_heading`=@maplink_heading, `maplink_color`=@maplink_color, `maplink_offset`=@maplink_offset, `maplink_width`=@maplink_width, `distance`=@distance, `left_x`=@left_x,  `left_y`=@left_y, `left_z`=@left_z, `right_x`=@right_x, `right_y`=@right_y, `right_z`=@right_z, `invertedPos`=@invertedPos, `to_x`=@to_x, `to_y`=@to_y, `to_z`=@to_z,`to_heading`=@to_heading,`state`=@state,`created`=@created,`updated`=@updated WHERE `id`=@id;";

        private const string _SqlDeleteMapTransition =
            "DELETE FROM `nec_map_transition` WHERE `id`=@id;";

        public bool InsertMapTransition(MapTransition mapTran)
        {
            int rowsAffected = ExecuteNonQuery(_SqlInsertMapTransition, command =>
            {
                //AddParameter(command, "@id", gimmick.Id);
                AddParameter(command, "@map_id", mapTran.mapId);
                AddParameter(command, "@transition_map_id", mapTran.transitionMapId);
                AddParameter(command, "@x", mapTran.referencePos.X);
                AddParameter(command, "@y", mapTran.referencePos.Y);
                AddParameter(command, "@z", mapTran.referencePos.Z);
                AddParameter(command, "@maplink_heading", mapTran.maplinkHeading);
                AddParameter(command, "@maplink_color", mapTran.maplinkColor);
                AddParameter(command, "@maplink_offset", mapTran.maplinkOffset);
                AddParameter(command, "@maplink_width", mapTran.maplinkWidth);
                AddParameter(command, "@distance", mapTran.refDistance);
                AddParameter(command, "@left_x", mapTran.leftPos.X);
                AddParameter(command, "@left_y", mapTran.leftPos.Y);
                AddParameter(command, "@left_z", mapTran.leftPos.Z);
                AddParameter(command, "@right_x", mapTran.rightPos.X);
                AddParameter(command, "@right_y", mapTran.rightPos.Y);
                AddParameter(command, "@right_z", mapTran.rightPos.Z);
                AddParameter(command, "@invertedPos", mapTran.invertedTransition);
                AddParameter(command, "@to_x", mapTran.toPos.x);
                AddParameter(command, "@to_y", mapTran.toPos.y);
                AddParameter(command, "@to_z", mapTran.toPos.z);
                AddParameter(command, "@to_heading", mapTran.toPos.heading);
                AddParameter(command, "@state", mapTran.state);
                AddParameter(command, "@created", mapTran.created);
                AddParameter(command, "@updated", mapTran.updated);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            mapTran.id = (int) autoIncrement;
            return true;
        }

        public List<MapTransition> SelectMapTransitions()
        {
            List<MapTransition> mapTrans = new List<MapTransition>();
            ExecuteReader(_SqlSelectMapTransitions, reader =>
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
            ExecuteReader(_SqlSelectMapTransitionsById,
                command => { AddParameter(command, "@id", id); },
                reader =>
                {
                    if (reader.Read())
                    {
                        mapTrans = ReadMapTransition(reader);
                    }
                });
            return mapTrans;
        }
        public List<MapTransition> SelectMapTransitionsByMapId(int mapId)
        {
            List<MapTransition> mapTrans = new List<MapTransition>();
            ExecuteReader(_SqlSelectMapTransitionsByMapId,
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
            int rowsAffected = ExecuteNonQuery(_SqlUpdateMapTransition, command =>
            {
                AddParameter(command, "@id", mapTran.id);
                AddParameter(command, "@map_id", mapTran.mapId);
                AddParameter(command, "@transition_map_id", mapTran.transitionMapId);
                AddParameter(command, "@x", mapTran.referencePos.X);
                AddParameter(command, "@y", mapTran.referencePos.Y);
                AddParameter(command, "@z", mapTran.referencePos.Z);
                AddParameter(command, "@maplink_heading", mapTran.maplinkHeading);
                AddParameter(command, "@maplink_color", mapTran.maplinkColor);
                AddParameter(command, "@maplink_offset", mapTran.maplinkOffset);
                AddParameter(command, "@maplink_width", mapTran.maplinkWidth);
                AddParameter(command, "@distance", mapTran.refDistance);
                AddParameter(command, "@left_x", mapTran.leftPos.X);
                AddParameter(command, "@left_y", mapTran.leftPos.Y);
                AddParameter(command, "@left_z", mapTran.leftPos.Z);
                AddParameter(command, "@right_x", mapTran.rightPos.X);
                AddParameter(command, "@right_y", mapTran.rightPos.Y);
                AddParameter(command, "@right_z", mapTran.rightPos.Z);
                AddParameter(command, "@invertedPos", mapTran.invertedTransition);
                AddParameter(command, "@to_x", mapTran.toPos.x);
                AddParameter(command, "@to_y", mapTran.toPos.y);
                AddParameter(command, "@to_z", mapTran.toPos.z);
                AddParameter(command, "@to_heading", mapTran.toPos.heading);
                AddParameter(command, "@state", mapTran.state);
                AddParameter(command, "@created", mapTran.created);
                AddParameter(command, "@updated", mapTran.updated);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteMapTransition(int mapTranId)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteMapTransition,
                command => { AddParameter(command, "@id", mapTranId); });
            return rowsAffected > NoRowsAffected;
        }

        private MapTransition ReadMapTransition(DbDataReader reader)
        {
            MapTransition mapTran = new MapTransition();
            mapTran.id = GetInt32(reader, "id");
            mapTran.mapId = GetInt32(reader, "map_id");
            mapTran.transitionMapId = GetInt32(reader, "transition_map_id");
            mapTran.referencePos.X = GetFloat(reader, "x");
            mapTran.referencePos.Y = GetFloat(reader, "y");
            mapTran.referencePos.Z = GetFloat(reader, "z");
            mapTran.maplinkHeading = GetByte(reader, "maplink_heading");
            mapTran.maplinkColor = GetInt32(reader, "maplink_color");
            mapTran.maplinkOffset = GetInt32(reader, "maplink_offset");
            mapTran.maplinkWidth = GetInt32(reader, "maplink_width");
            mapTran.refDistance = GetInt32(reader, "distance");
            mapTran.leftPos.X = GetFloat(reader, "left_x");
            mapTran.leftPos.Y = GetFloat(reader, "left_y");
            mapTran.leftPos.Z = GetFloat(reader, "left_z");
            mapTran.rightPos.X = GetFloat(reader, "right_x");
            mapTran.rightPos.Y = GetFloat(reader, "right_y");
            mapTran.rightPos.Z = GetFloat(reader, "right_z");
            mapTran.invertedTransition = GetBoolean(reader, "invertedPos");
            mapTran.toPos.x = GetFloat(reader, "to_x");
            mapTran.toPos.y = GetFloat(reader, "to_y");
            mapTran.toPos.z = GetFloat(reader, "to_z");
            mapTran.toPos.heading = GetByte(reader, "to_heading");
            mapTran.state = GetInt32(reader, "state") == 0 ? false : true;
            mapTran.created = GetDateTime(reader, "created");
            mapTran.updated = GetDateTime(reader, "updated");
            return mapTran;
        }
    }
}
