using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model.MapModel;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_CREATE_MAP =
            "INSERT INTO `nec_map` (`id`, `country`, `area`, `place`, `x`, `y`, `z`, `orientation`) VALUES (@id, @country, @area, @place, @x, @y, @z, @orientation);";

        private const string SQL_SELECT_MAP_BY_ID =
            "SELECT `id`, `country`, `area`, `place`, `x`, `y`, `z`, `orientation` FROM `nec_map` WHERE `id`=@id; ";

        private const string SQL_SELECT_MAPS =
            "SELECT `id`, `country`, `area`, `place`, `x`, `y`, `z`, `orientation` FROM `nec_map`; ";

        private const string SQL_UPDATE_MAP =
            "UPDATE `nec_map` SET `country`=@country, `area`=@area, `place`=@place, `x`=@x, `y`=@y, `z`=@z `orientation`=@orientation WHERE `id`=@id;";

        private const string SQL_DELETE_MAP =
            "DELETE FROM `nec_map` WHERE `id`=@id;";

        public bool InsertMap(MapData map)
        {
            int rowsAffected = ExecuteNonQuery(SQL_CREATE_MAP, command =>
            {
                AddParameter(command, "@id", map.id);
                AddParameter(command, "@country", map.country);
                AddParameter(command, "@area", map.area);
                AddParameter(command, "@place", map.place);
                AddParameter(command, "@x", map.x);
                AddParameter(command, "@y", map.y);
                AddParameter(command, "@z", map.z);
                AddParameter(command, "@orientation", map.orientation);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED) return false;

            return true;
        }

        public MapData SelectItemMapId(int mapId)
        {
            MapData map = null;
            ExecuteReader(SQL_SELECT_MAP_BY_ID,
                command => { AddParameter(command, "@id", mapId); }, reader =>
                {
                    if (reader.Read()) map = ReadMap(reader);
                });
            return map;
        }

        public List<MapData> SelectMaps()
        {
            List<MapData> maps = new List<MapData>();
            ExecuteReader(SQL_SELECT_MAPS, reader =>
            {
                while (reader.Read())
                {
                    MapData map = ReadMap(reader);
                    maps.Add(map);
                }
            });
            return maps;
        }

        public bool UpdateMap(MapData map)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_MAP, command =>
            {
                AddParameter(command, "@id", map.id);
                AddParameter(command, "@country", map.country);
                AddParameter(command, "@area", map.area);
                AddParameter(command, "@place", map.place);
                AddParameter(command, "@x", map.x);
                AddParameter(command, "@y", map.y);
                AddParameter(command, "@z", map.z);
                AddParameter(command, "@orientation", map.orientation);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteMap(int mapId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_MAP,
                command => { AddParameter(command, "@id", mapId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        private MapData ReadMap(DbDataReader reader)
        {
            MapData map = new MapData();
            map.id = GetInt32(reader, "id");
            map.country = GetString(reader, "country");
            map.area = GetString(reader, "area");
            map.place = GetString(reader, "place");
            map.x = GetInt32(reader, "x");
            map.y = GetInt32(reader, "y");
            map.z = GetInt32(reader, "z");
            map.orientation = GetInt32(reader, "orientation");
            return map;
        }
    }
}
