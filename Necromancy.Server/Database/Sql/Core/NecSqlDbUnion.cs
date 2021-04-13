using System.Data.Common;
using Necromancy.Server.Model.Union;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_INSERT_UNION =
            "INSERT INTO `nec_union` (`name`,`leader_character_id`,`subleader1_character_id`,`subleader2_character_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created`)VALUES(@name,@leader_character_id,@subleader1_character_id,@subleader2_character_id,@level,@current_exp,@next_level_exp,@member_limit_increase,@cape_design_id,@union_news,@created);";

        private const string SQL_SELECT_UNION_BY_ID =
            "SELECT `id`,`name`,`leader_character_id`,`subleader1_character_id`,`subleader2_character_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created` FROM `nec_union` WHERE `id`=@id;";

        private const string SQL_SELECT_UNION_BY_LEADER_ID =
            "SELECT `id`,`name`,`leader_character_id`,`subleader1_character_id`,`subleader2_character_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created` FROM `nec_union` WHERE `leader_character_id`=@leader_character_id;";

        private const string SQL_SELECT_UNION_BY_NAME =
            "SELECT `id`,`name`,`leader_character_id`,`subleader1_character_id`,`subleader2_character_id`,`level`,`current_exp`,`next_level_exp`,`member_limit_increase`,`cape_design_id`,`union_news`,`created` FROM `nec_union` WHERE `name`=@name;";


        private const string SQL_UPDATE_UNION =
            "UPDATE `nec_union` SET `id`=@id,`name`=@name,`leader_character_id`=@leader_character_id,`subleader1_character_id`=@subleader1_character_id,`subleader2_character_id`=@subleader2_character_id,`level`=@level,`current_exp`=@current_exp,`next_level_exp`=@next_level_exp,`member_limit_increase`=@member_limit_increase,`cape_design_id`=@cape_design_id,`union_news`=@union_news,`created`=@created WHERE `id`=@id;";

        private const string SQL_DELETE_UNION =
            "DELETE FROM `nec_union` WHERE `id`=@id;";

        public bool InsertUnion(Union union)
        {
            int rowsAffected = ExecuteNonQuery(SQL_INSERT_UNION, command =>
            {
                //AddParameter(command, "@id", union.Id);
                AddParameter(command, "@name", union.name);
                AddParameter(command, "@leader_character_id", union.leaderId);
                AddParameter(command, "@subleader1_character_id", union.subLeader1Id);
                AddParameter(command, "@subleader2_character_id", union.subLeader2Id);
                AddParameter(command, "@level", union.level);
                AddParameter(command, "@current_exp", union.currentExp);
                AddParameter(command, "@next_level_exp", union.nextLevelExp);
                AddParameter(command, "@member_limit_increase", union.memberLimitIncrease);
                AddParameter(command, "@cape_design_id", union.capeDesignId);
                AddParameter(command, "@union_news", union.unionNews);
                AddParameter(command, "@created", union.created);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED || autoIncrement <= NO_AUTO_INCREMENT) return false;

            union.id = (int)autoIncrement;
            return true;
        }

        public Union SelectUnionById(int unionId)
        {
            Union union = null;
            ExecuteReader(SQL_SELECT_UNION_BY_ID,
                command => { AddParameter(command, "@id", unionId); }, reader =>
                {
                    if (reader.Read()) union = ReadUnion(reader);
                });
            return union;
        }

        public Union SelectUnionByLeaderId(int leaderId)
        {
            Union union = null;
            ExecuteReader(SQL_SELECT_UNION_BY_LEADER_ID,
                command => { AddParameter(command, "@leader_character_id", leaderId); }, reader =>
                {
                    if (reader.Read()) union = ReadUnion(reader);
                });
            return union;
        }

        public Union SelectUnionByName(string unionName)
        {
            Union union = null;
            ExecuteReader(SQL_SELECT_UNION_BY_NAME,
                command => { AddParameter(command, "@name", unionName); }, reader =>
                {
                    if (reader.Read()) union = ReadUnion(reader);
                });
            return union;
        }

        public bool UpdateUnion(Union union)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_UNION, command =>
            {
                AddParameter(command, "@id", union.id);
                AddParameter(command, "@name", union.name);
                AddParameter(command, "@leader_character_id", union.leaderId);
                AddParameter(command, "@subleader1_character_id", union.subLeader1Id);
                AddParameter(command, "@subleader2_character_id", union.subLeader2Id);
                AddParameter(command, "@level", union.level);
                AddParameter(command, "@current_exp", union.currentExp);
                AddParameter(command, "@next_level_exp", union.nextLevelExp);
                AddParameter(command, "@member_limit_increase", union.memberLimitIncrease);
                AddParameter(command, "@cape_design_id", union.capeDesignId);
                AddParameter(command, "@union_news", union.unionNews);
                AddParameter(command, "@created", union.created);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteUnion(int unionId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_UNION, command => { AddParameter(command, "@id", unionId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        private Union ReadUnion(DbDataReader reader)
        {
            {
                Union union = new Union();
                union.id = GetInt32(reader, "id");
                union.name = GetStringNullable(reader, "name");
                union.leaderId = GetInt32(reader, "leader_character_id");
                union.subLeader1Id = GetInt32(reader, "subleader1_character_id");
                union.subLeader2Id = GetInt32(reader, "subleader1_character_id");
                union.level = (uint)GetInt32(reader, "level");
                union.currentExp = (uint)GetInt32(reader, "current_exp");
                union.nextLevelExp = (uint)GetInt32(reader, "next_level_exp");
                union.memberLimitIncrease = (byte)GetInt32(reader, "member_limit_increase");
                union.capeDesignId = GetInt16(reader, "cape_design_id");
                union.unionNews = GetStringNullable(reader, "union_news");
                union.created = GetDateTime(reader, "created");
                return union;
            }
        }
    }
}
