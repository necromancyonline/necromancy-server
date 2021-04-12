using System.Collections.Generic;
using System.Data.Common;
using System.Numerics;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_INSERT_MONSTER_COORD =
            "INSERT INTO `nec_monster_coords` (`monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z`) VALUES (@monster_id, @map_id, @coord_idx, @x, @y, @z);";

        private const string SQL_SELECT_MONSTER_COORDS =
            "SELECT `id`, `monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z` FROM `nec_monster_coords`;";

        private const string SQL_SELECT_MONSTER_COORD_BY_MAP_ID =
            "SELECT `id`, `monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z` FROM `nec_monster_coords` WHERE `map_id`=@map_id;";

        private const string SQL_SELECT_MONSTER_COORD_BY_ID =
            "SELECT `id`, `monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z` FROM `nec_monster_coords` WHERE `id`=@id;";

        private const string SQL_SELECT_MONSTER_COORD_BY_MONSTER_ID =
            "SELECT `id`, `monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z` FROM `nec_monster_coords` WHERE `monster_id`=@monster_id;";

        private const string SQL_UPDATE_MONSTER_COORD =
            "UPDATE `nec_monster_coords` SET `monster_id`=@monster_id, `map_id`=@map_id, `coord_idx`=@coord_idx, `x`=@x, `y`=@y, `z`=@z WHERE `id`=@id;";

        private const string SQL_DELETE_MONSTER_COORD =
            "DELETE FROM `nec_monster_coords` WHERE `id`=@id;";

        public bool InsertMonsterCoords(MonsterCoord monsterCoord)
        {
            int rowsAffected = ExecuteNonQuery(SQL_INSERT_MONSTER_COORD, command =>
            {
                AddParameter(command, "@monster_id", monsterCoord.monsterId);
                AddParameter(command, "@map_id", monsterCoord.mapId);
                AddParameter(command, "@coord_idx", monsterCoord.coordIdx);
                AddParameter(command, "@x", monsterCoord.destination.X);
                AddParameter(command, "@y", monsterCoord.destination.Y);
                AddParameter(command, "@z", monsterCoord.destination.Z);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED || autoIncrement <= NO_AUTO_INCREMENT) return false;

            monsterCoord.Id = (int)autoIncrement;
            return true;
        }

        public List<MonsterCoord> SelectMonsterCoords()
        {
            List<MonsterCoord> monsterCoords = new List<MonsterCoord>();
            ExecuteReader(SQL_SELECT_MONSTER_COORDS, reader =>
            {
                while (reader.Read())
                {
                    MonsterCoord monsterCoord = ReadMonsterCoord(reader);
                    monsterCoords.Add(monsterCoord);
                }
            });
            return monsterCoords;
        }

        public List<MonsterCoord> SelectMonsterCoordsById(int id)
        {
            List<MonsterCoord> monsterCoords = new List<MonsterCoord>();
            ExecuteReader(SQL_SELECT_MONSTER_COORD_BY_ID,
                command => { AddParameter(command, "@id", id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        MonsterCoord monsterCoord = ReadMonsterCoord(reader);
                        monsterCoords.Add(monsterCoord);
                    }
                });
            return monsterCoords;
        }

        public List<MonsterCoord> SelectMonsterCoordsByMonsterId(int mapId)
        {
            List<MonsterCoord> monsterCoords = new List<MonsterCoord>();
            ExecuteReader(SQL_SELECT_MONSTER_COORD_BY_MONSTER_ID,
                command => { AddParameter(command, "@monster_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        MonsterCoord monsterCoord = ReadMonsterCoord(reader);
                        monsterCoords.Add(monsterCoord);
                    }
                });
            return monsterCoords;
        }

        public List<MonsterCoord> SelectMonsterCoordsByMapId(int mapId)
        {
            List<MonsterCoord> monsterCoords = new List<MonsterCoord>();
            ExecuteReader(SQL_SELECT_MONSTER_COORD_BY_MAP_ID,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        MonsterCoord monsterCoord = ReadMonsterCoord(reader);
                        monsterCoords.Add(monsterCoord);
                    }
                });
            return monsterCoords;
        }

        public bool UpdateMonsterCoord(MonsterCoord monsterCoord)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_MONSTER_COORD, command =>
            {
                AddParameter(command, "@monster_id", monsterCoord.monsterId);
                AddParameter(command, "@map_id", monsterCoord.mapId);
                AddParameter(command, "@coord_idx", monsterCoord.coordIdx);
                AddParameter(command, "@x", monsterCoord.destination.X);
                AddParameter(command, "@y", monsterCoord.destination.Y);
                AddParameter(command, "@z", monsterCoord.destination.Z);
                AddParameter(command, "@id", monsterCoord.Id);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteMonsterCoord(int coordId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_MONSTER_COORD,
                command => { AddParameter(command, "@id", coordId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        private MonsterCoord ReadMonsterCoord(DbDataReader reader)
        {
            MonsterCoord monsterCoord = new MonsterCoord();
            Vector3 coords = new Vector3();
            monsterCoord.Id = GetInt32(reader, "id");
            monsterCoord.monsterId = (uint)GetInt32(reader, "monster_id");
            monsterCoord.mapId = (uint)GetInt32(reader, "map_id");
            monsterCoord.coordIdx = GetInt32(reader, "coord_idx");
            coords.X = GetFloat(reader, "x");
            coords.Y = GetFloat(reader, "y");
            coords.Z = GetFloat(reader, "z");
            monsterCoord.destination = coords;
            return monsterCoord;
        }
    }
}
