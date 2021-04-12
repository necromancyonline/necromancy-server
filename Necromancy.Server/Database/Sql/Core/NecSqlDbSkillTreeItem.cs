using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_INSERT_SKILL_TREE_ITEM =
            "INSERT INTO `nec_skilltree_item` (`skill_id`, `char_id`, `level`) VALUES (@skill_id, @char_id, @level);";

        private const string SQL_SELECT_SKILL_TREE_ITEM_BY_ID =
            "SELECT `id`, `skill_id`, `char_id`, `level` FROM `nec_skilltree_item` WHERE `id`=@id;";

        private const string SQL_SELECT_SKILL_TREE_ITEMS_BY_CHAR_ID =
            "SELECT `id`, `skill_id`, `char_id`, `level` FROM `nec_skilltree_item` WHERE `char_id`=@char_id;";

        private const string SQL_SELECT_SKILL_TREE_ITEM_BY_CHAR_SKILL_ID =
            "SELECT `id`, `skill_id`, `char_id`, `level` FROM `nec_skilltree_item` WHERE `char_id`=@char_id AND `skill_id`=@skill_id;";

        private const string SQL_UPDATE_SKILL_TREE_ITEM =
            "UPDATE `nec_skilltree_item` SET `skill_id`=@skill_id, `char_id`=@char_id, `level`=@level WHERE `id`=@id;";

        private const string SQL_DELETE_SKILL_TREE_ITEM =
            "DELETE FROM `nec_skilltree_item` WHERE `id`=@id;";

        public bool InsertSkillTreeItem(SkillTreeItem skillTreeItem)
        {
            int rowsAffected = ExecuteNonQuery(SQL_INSERT_SKILL_TREE_ITEM, command =>
            {
                AddParameter(command, "@id", skillTreeItem.id);
                AddParameter(command, "@skill_id", skillTreeItem.skillId);
                AddParameter(command, "@char_id", skillTreeItem.charId);
                AddParameter(command, "@level", skillTreeItem.level);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED || autoIncrement <= NO_AUTO_INCREMENT) return false;
            skillTreeItem.id = (int)autoIncrement;
            return true;
        }

        public SkillTreeItem SelectSkillTreeItemById(int id)
        {
            SkillTreeItem skillTreeItem = null;
            ExecuteReader(SQL_SELECT_SKILL_TREE_ITEM_BY_ID,
                command => { AddParameter(command, "@id", id); }, reader =>
                {
                    if (reader.Read()) skillTreeItem = ReadSkillTreeItem(reader);
                });
            return skillTreeItem;
        }

        public List<SkillTreeItem> SelectSkillTreeItemsByCharId(int charId)
        {
            List<SkillTreeItem> skillTreeItems = new List<SkillTreeItem>();
            ExecuteReader(SQL_SELECT_SKILL_TREE_ITEMS_BY_CHAR_ID,
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
            ExecuteReader(SQL_SELECT_SKILL_TREE_ITEM_BY_CHAR_SKILL_ID,
                command =>
                {
                    AddParameter(command, "@char_id", charId);
                    AddParameter(command, "@skill_id", skillId);
                }, reader =>
                {
                    if (reader.Read()) skillTreeItem = ReadSkillTreeItem(reader);
                });
            return skillTreeItem;
        }

        public bool UpdateSkillTreeItem(SkillTreeItem skillTreeItem)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_SKILL_TREE_ITEM, command =>
            {
                AddParameter(command, "@skill_id", skillTreeItem.skillId);
                AddParameter(command, "@char_id", skillTreeItem.charId);
                AddParameter(command, "@level", skillTreeItem.level);
                AddParameter(command, "@id", skillTreeItem.id);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteSkillTreeItem(int id)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_SKILL_TREE_ITEM, command => { AddParameter(command, "@id", id); });
            return rowsAffected > NO_ROWS_AFFECTED;
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
