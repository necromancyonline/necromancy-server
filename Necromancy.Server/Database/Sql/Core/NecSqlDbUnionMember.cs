using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model.Union;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_INSERT_UNION_MEMBER =
            "INSERT INTO `nec_union_member` (`union_id`,`character_id`,`member_priviledge_bitmask`,`rank`,`joined`)VALUES(@union_id,@character_id,@member_priviledge_bitmask,@rank,@joined);";

        private const string SQL_SELECT_UNION_MEMBER_BY_CHARACTER_ID =
            "SELECT `id`,`union_id`,`character_id`,`member_priviledge_bitmask`,`rank`,`joined` FROM `nec_union_member` WHERE `character_id`=@character_id;";

        private const string SQL_SELECT_UNION_MEMBERS_BY_UNION_ID =
            "SELECT `id`,`union_id`,`character_id`,`member_priviledge_bitmask`,`rank`,`joined` FROM `nec_union_member` WHERE `union_id`=@union_id;";

        private const string SQL_UPDATE_UNION_MEMBER =
            "UPDATE `nec_union_member` SET `union_id`=@union_id,`character_id`=@character_id,`member_priviledge_bitmask`=@member_priviledge_bitmask,`rank`=@rank,`joined`=@joined  WHERE `character_id`=@character_id;";

        private const string SQL_DELETE_UNION_MEMBER =
            "DELETE FROM `nec_union_member` WHERE `character_id`=@character_id;";

        private const string SQL_DELETE_ALL_UNION_MEMBER =
            "DELETE FROM `nec_union_member` WHERE `union_id`=@union_id;";

        public bool InsertUnionMember(UnionMember unionMember)
        {
            int rowsAffected = ExecuteNonQuery(SQL_INSERT_UNION_MEMBER, command =>
            {
                //AddParameter(command, "@id", unionMember.Id);
                AddParameter(command, "@union_id", unionMember.unionId);
                AddParameter(command, "@character_id", unionMember.characterDatabaseId);
                AddParameter(command, "@member_priviledge_bitmask", unionMember.memberPriviledgeBitMask);
                AddParameter(command, "@rank", unionMember.rank);
                AddParameter(command, "@joined", unionMember.joined);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED || autoIncrement <= NO_AUTO_INCREMENT) return false;

            unionMember.id = (int)autoIncrement;
            return true;
        }

        public UnionMember SelectUnionMemberByCharacterId(int characterDatabaseId)
        {
            UnionMember unionMember = null;
            ExecuteReader(SQL_SELECT_UNION_MEMBER_BY_CHARACTER_ID,
                command => { AddParameter(command, "@character_id", characterDatabaseId); }, reader =>
                {
                    if (reader.Read()) unionMember = ReadUnionMember(reader);
                });
            return unionMember;
        }

        public List<UnionMember> SelectUnionMembersByUnionId(int unionId)
        {
            List<UnionMember> unionMembers = new List<UnionMember>();
            ExecuteReader(SQL_SELECT_UNION_MEMBERS_BY_UNION_ID,
                command => { AddParameter(command, "@union_id", unionId); }, reader =>
                {
                    while (reader.Read())
                    {
                        UnionMember unionMember = ReadUnionMember(reader);
                        unionMembers.Add(unionMember);
                    }
                });
            return unionMembers;
        }

        public bool UpdateUnionMember(UnionMember unionMember)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_UNION_MEMBER, command =>
            {
                AddParameter(command, "@id", unionMember.id);
                AddParameter(command, "@union_id", unionMember.unionId);
                AddParameter(command, "@character_id", unionMember.characterDatabaseId);
                AddParameter(command, "@member_priviledge_bitmask", unionMember.memberPriviledgeBitMask);
                AddParameter(command, "@rank", unionMember.rank);
                AddParameter(command, "@joined", unionMember.joined);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteUnionMember(int characterDatabaseId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_UNION_MEMBER, command => { AddParameter(command, "@character_id", characterDatabaseId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteAllUnionMembers(int unionId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_ALL_UNION_MEMBER, command => { AddParameter(command, "@union_id", unionId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        private UnionMember ReadUnionMember(DbDataReader reader)
        {
            {
                UnionMember unionMember = new UnionMember();
                unionMember.id = GetInt32(reader, "id");
                unionMember.unionId = GetInt32(reader, "union_id");
                unionMember.characterDatabaseId = GetInt32(reader, "character_id");
                unionMember.memberPriviledgeBitMask = (uint)GetInt32(reader, "member_priviledge_bitmask");
                unionMember.rank = (uint)GetInt32(reader, "rank");
                unionMember.joined = GetDateTime(reader, "joined");
                return unionMember;
            }
        }
    }
}
