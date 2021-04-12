using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SQL_CREATE_QUEST =
            "INSERT INTO `QuestRequest` (`quest_id `, `soul_level_mission `, `quest_name `, `quest_level `, `time_limit `, `quest_giver_name `, `reward_exp `, `reward_gold `, `numbers_of_items `, `items_type`) VALUES (@quest_id, @soul_level_mission, @quest_name, @quest_level, @time_limit, @quest_giver_name, @reward_exp, @reward_gold, @numbers_of_items, @items_type);";

        private const string SQL_SELECT_QUEST_BY_ID =
            "SELECT `quest_id `, `soul_level_mission `, `quest_name `, `quest_level `, `time_limit `, `quest_giver_name `, `reward_exp `, `reward_gold `, `numbers_of_items `, `items_type` FROM `QuestRequest` WHERE `quest_id`=@quest_id; ";

        private const string SQL_UPDATE_QUEST =
            "UPDATE `QuestRequest` SET `id`=@id, `item_name`=@item_name, `items_type`=@items_type,  `physics`=@physics, `magic`=@magic, `enchant_id`=@enchant_id, `durab`=@durab, `hardness`=@hardness, `max_dur`=@max_dur, `numbers`=@numbers, `level`=@level, `sp_level`=@sp_level, `weight`=@weight, `state`=@state WHERE `id`=@id;";

        private const string SQL_DELETE_QUEST =
            "DELETE FROM `QuestRequest` WHERE `quest_id`=@quest_id;";

        public bool InsertQuest(Quest quest)
        {
            int rowsAffected = ExecuteNonQuery(SQL_CREATE_QUEST, command =>
            {
                AddParameter(command, "@quest_id", quest.questId);
                AddParameter(command, "@soul_level_mission", quest.soulLevelMission);
                AddParameter(command, "@quest_name", quest.questName);
                AddParameter(command, "@quest_level", quest.questLevel);
                AddParameter(command, "@time_limit", quest.timeLimit);
                AddParameter(command, "@quest_giver_name", quest.questGiverName);
                AddParameter(command, "@reward_exp", quest.rewardExp);
                AddParameter(command, "@reward_gold", quest.rewardGold);
                AddParameter(command, "@numbers_of_items", quest.numbersOfItems);
                AddParameter(command, "@items_type", quest.itemsType);
            }, out long autoIncrement);
            if (rowsAffected <= NO_ROWS_AFFECTED || autoIncrement <= NO_AUTO_INCREMENT) return false;

            quest.questId = (int)autoIncrement;
            return true;
        }


        public Quest SelectQuestById(int questId)
        {
            Quest quest = null;
            ExecuteReader(SQL_SELECT_QUEST_BY_ID,
                command => { AddParameter(command, "@quest_id", questId); }, reader =>
                {
                    if (reader.Read()) quest = ReadQuest(reader);
                });
            return quest;
        }

        public bool UpdateQuest(Quest quest)
        {
            int rowsAffected = ExecuteNonQuery(SQL_UPDATE_QUEST, command =>
            {
                AddParameter(command, "@quest_id", quest.questId);
                AddParameter(command, "@soul_level_mission", quest.soulLevelMission);
                AddParameter(command, "@quest_name", quest.questName);
                AddParameter(command, "@quest_level", quest.questLevel);
                AddParameter(command, "@time_limit", quest.timeLimit);
                AddParameter(command, "@quest_giver_name", quest.questGiverName);
                AddParameter(command, "@reward_exp", quest.rewardExp);
                AddParameter(command, "@reward_gold", quest.rewardGold);
                AddParameter(command, "@numbers_of_items", quest.numbersOfItems);
                AddParameter(command, "@items_type", quest.itemsType);
            });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        public bool DeleteQuest(int questId)
        {
            int rowsAffected = ExecuteNonQuery(SQL_DELETE_QUEST,
                command => { AddParameter(command, "@quest_id", questId); });
            return rowsAffected > NO_ROWS_AFFECTED;
        }

        private Quest ReadQuest(DbDataReader reader)
        {
            Quest quest = new Quest();
            quest.questId = GetInt32(reader, "quest_id");
            quest.soulLevelMission = GetByte(reader, "soul_level_mission");
            quest.questName = GetString(reader, "quest_name");
            quest.questLevel = GetInt32(reader, "quest_level");
            quest.timeLimit = GetInt32(reader, "time_limit");
            quest.questGiverName = GetString(reader, "quest_giver_name");
            quest.rewardExp = GetInt32(reader, "reward_exp");
            quest.rewardGold = GetInt32(reader, "reward_gold");
            quest.numbersOfItems = (short)GetInt32(reader, "numbers_of_items");
            quest.itemsType = GetInt32(reader, "items_type");
            return quest;
        }
    }
}
