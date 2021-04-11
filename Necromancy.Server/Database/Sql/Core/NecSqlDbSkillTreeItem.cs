using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string _SqlInsertSkillTreeItem =
            "INSERT INTO `nec_skilltree_item` (`skill_id`, `char_id`, `level`) VALUES (@skill_id, @char_id, @level);";

        private const string _SqlSelectSkillTreeItemById =
            "SELECT `id`, `skill_id`, `char_id`, `level` FROM `nec_skilltree_item` WHERE `id`=@id;";

        private const string _SqlSelectSkillTreeItemsByCharId =
            "SELECT `id`, `skill_id`, `char_id`, `level` FROM `nec_skilltree_item` WHERE `char_id`=@char_id;";

        private const string _SqlSelectSkillTreeItemByCharSkillId =
            "SELECT `id`, `skill_id`, `char_id`, `level` FROM `nec_skilltree_item` WHERE `char_id`=@char_id AND `skill_id`=@skill_id;";

        private const string _SqlUpdateSkillTreeItem =
            "UPDATE `nec_skilltree_item` SET `skill_id`=@skill_id, `char_id`=@char_id, `level`=@level WHERE `id`=@id;";

        private const string _SqlDeleteSkillTreeItem =
            "DELETE FROM `nec_skilltree_item` WHERE `id`=@id;";

        public bool InsertSkillTreeItem(SkillTreeItem skillTreeItem)
        {
            int rowsAffected = ExecuteNonQuery(_SqlInsertSkillTreeItem, command =>
            {
                AddParameter(command, "@id", skillTreeItem.id);
                AddParameter(command, "@skill_id", skillTreeItem.skillId);
                AddParameter(command, "@char_id", skillTreeItem.charId);
                AddParameter(command, "@level", skillTreeItem.level);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }
            skillTreeItem.id = (int)autoIncrement;
            return true;
        }

        public SkillTreeItem SelectSkillTreeItemById(int id)
        {
            SkillTreeItem skillTreeItem = null;
            ExecuteReader(_SqlSelectSkillTreeItemById,
                command => { AddParameter(command, "@id", id); }, reader =>
                {
                    if (reader.Read())
                    {
                        skillTreeItem = ReadSkillTreeItem(reader);
                    }
                });
            return skillTreeItem;
        }

        public List<SkillTreeItem> SelectSkillTreeItemsByCharId(int charId)
        {
            List<SkillTreeItem> skillTreeItems = new List<SkillTreeItem>();
            ExecuteReader(_SqlSelectSkillTreeItemsByCharId,
                command => { AddParameter(command, "@char_id", charId); }, reader =>
                {
                    while (reader.Read())
                    {
                        SkillTreeItem skillTreeItem = ReadSkillTreeItem(reader);
                        skillTreeItems.Add(skillTreeItem);
                    }
                });
            return skillTreeItems;
        }

        public SkillTreeItem SelectSkillTreeItemByCharSkillId(int charId, int skillId)
        {
            SkillTreeItem skillTreeItem = null;
            ExecuteReader(_SqlSelectSkillTreeItemByCharSkillId,
                command => {
                    AddParameter(command, "@char_id", charId);
                    AddParameter(command, "@skill_id", skillId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        skillTreeItem = ReadSkillTreeItem(reader);
                    }
                });
            return skillTreeItem;
        }
        public bool UpdateSkillTreeItem(SkillTreeItem skillTreeItem)
        {
            int rowsAffected = ExecuteNonQuery(_SqlUpdateSkillTreeItem, command =>
            {
                AddParameter(command, "@skill_id", skillTreeItem.skillId);
                AddParameter(command, "@char_id", skillTreeItem.charId);
                AddParameter(command, "@level", skillTreeItem.level);
                AddParameter(command, "@id", skillTreeItem.id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteSkillTreeItem(int id)
        {
            int rowsAffected = ExecuteNonQuery(_SqlDeleteSkillTreeItem, command => { AddParameter(command, "@id", id); });
            return rowsAffected > NoRowsAffected;
        }

        private SkillTreeItem ReadSkillTreeItem(DbDataReader reader)
        {
            SkillTreeItem skillTreeItem = new SkillTreeItem();
            skillTreeItem.id = GetInt32(reader, "id");
            skillTreeItem.skillId = GetInt32(reader, "skill_id");
            skillTreeItem.charId = GetInt32(reader, "char_id");
            skillTreeItem.level = GetInt32(reader, "level");
            return skillTreeItem;
        }
    }
}
