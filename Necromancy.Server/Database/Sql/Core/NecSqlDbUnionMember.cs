using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model.Union;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string _SqlInsertUnionMember =
            "INSERT INTO `nec_union_member` (`union_id`,`character_id`,`member_priviledge_bitmask`,`rank`,`joined`)VALUES(@union_id,@character_id,@member_priviledge_bitmask,@rank,@joined);";

        private const string _SqlSelectUnionMemberByCharacterId =
            "SELECT `id`,`union_id`,`character_id`,`member_priviledge_bitmask`,`rank`,`joined` FROM `nec_union_member` WHERE `character_id`=@character_id;";

        private const string _SqlSelectUnionMembersByUnionId =
            "SELECT `id`,`union_id`,`character_id`,`member_priviledge_bitmask`,`rank`,`joined` FROM `nec_union_member` WHERE `union_id`=@union_id;";

        private const string _SqlUpdateUnionMember =
            "UPDATE `nec_union_member` SET `union_id`=@union_id,`character_id`=@character_id,`member_priviledge_bitmask`=@member_priviledge_bitmask,`rank`=@rank,`joined`=@joined  WHERE `character_id`=@character_id;";

        private const string _SqlDeleteUnionMember =
            "DELETE FROM `nec_union_member` WHERE `character_id`=@character_id;";

        private const string _SqlDeleteAllUnionMember =
            "DELETE FROM `nec_union_member` WHERE `union_id`=@union_id;";

        public bool InsertUnionMember(UnionMember unionMember)
        {
            int rowsAffected = ExecuteNonQuery(_SqlInsertUnionMember, command =>
            {
                //AddParameter(command, "@id", unionMember.Id);
                AddParameter(command, "@union_id", unionMember.unionId);
                AddParameter(command, "@character_id", unionMember.characterDatabaseId);
                AddParameter(command, "@member_priviledge_bitmask", unionMember.memberPriviledgeBitMask);
                AddParameter(command, "@rank", unionMember.rank);
                AddParameter(command, "@joined", unionMember.joined);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement) return false;

            unionMember.id = (int)autoIncrement;
            return true;
        }

        public UnionMember SelectUnionMemberByCharacterId(int characterDatabaseId)
        {
            UnionMember unionMember = null;
            ExecuteReader(_SqlSelectUnionMemberByCharacterId,
                command => { AddParameter(command, "@character_id", characterDatabaseId); }, reader =>
                {
                    if (reader.Read()) unionMember = ReadUnionMember(reader);
                });
            return unionMember;
        }

        public List<UnionMember> SelectUnionMembersByUnionId(int unionId)
        {
            List<UnionMember> unionMembers = new List<UnionMember>();
            ExecuteReader(_SqlSelectUnionMembersByUnionId,
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
            int rowsAffected = ExecuteNonQuery(_SqlUpdateUnionMember, command =>
            {
                AddParameter(command, "@id", unionMember.id);
                AddParameter(command, "@union_id", unionMember.unionId);
                AddParameter(command, "@character_id", unionMember.characterDatabaseId);
                AddParameter(command, "@member_priviledge_bitmask", unionMember.memberPriviledgeBitMask);
                AddParameter(command, "@rank", unionMember.rank);
                AddParameter(command, "@joined", unionMember.joined);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteUnionMember(int characterDatabaseId)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteUnionMember, command => { AddParameter(command, "@character_id", characterDatabaseId); });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteAllUnionMembers(int unionId)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteAllUnionMember, command => { AddParameter(command, "@union_id", unionId); });
            return rowsAffected > NoRowsAffected;
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
