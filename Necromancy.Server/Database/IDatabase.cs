using System.Collections.Generic;
using Necromancy.Server.Model;
using Necromancy.Server.Model.MapModel;
using Necromancy.Server.Model.Union;

namespace Necromancy.Server.Database
{
    public interface IDatabase
    {
        const int INVALID_DATABASE_ID = -1;

        long version { get; set; }

        void Execute(string sql);

        /// <summary>
        ///     Return true if database was created, or false if not.
        /// </summary>
        bool CreateDatabase();

        // Account
        Account CreateAccount(string name, string mail, string hash);
        Account SelectAccountById(int accountId);
        Account SelectAccountByName(string accountName);
        bool UpdateAccount(Account account);
        bool DeleteAccount(int accountId);

        // Soul
        bool InsertSoul(Soul soul);
        Soul SelectSoulById(int soulId);
        Soul SelectSoulByName(string soulName);
        List<Soul> SelectSoulsByAccountId(int accountId);
        bool UpdateSoul(Soul soul);
        bool DeleteSoul(int soulId);

        // Map
        bool InsertMap(MapData map);
        MapData SelectItemMapId(int mapId);
        List<MapData> SelectMaps();
        bool UpdateMap(MapData map);
        bool DeleteMap(int mapId);

        // Character
        bool InsertCharacter(Character character);
        Character SelectCharacterById(int characterId);
        Character SelectCharacterBySlot(int soulId, int slot);
        List<Character> SelectCharactersByAccountId(int accountId);
        List<Character> SelectCharactersBySoulId(int soulId);
        bool UpdateCharacter(Character character);
        bool DeleteCharacter(int characterId);
        List<Character> SelectCharacters();

        // Npc Spawn
        bool InsertNpcSpawn(NpcSpawn npcSpawn);
        List<NpcSpawn> SelectNpcSpawns();
        List<NpcSpawn> SelectNpcSpawnsByMapId(int mapId);
        bool UpdateNpcSpawn(NpcSpawn npcSpawn);
        bool DeleteNpcSpawn(int npcSpawnId);

        // Monster Spawn
        bool InsertMonsterSpawn(MonsterSpawn monsterSpawn);
        List<MonsterSpawn> SelectMonsterSpawns();
        List<MonsterSpawn> SelectMonsterSpawnsByMapId(int mapId);
        bool UpdateMonsterSpawn(MonsterSpawn monsterSpawn);
        bool DeleteMonsterSpawn(int monsterSpawnId);

        // ShortcutBar
        void InsertOrReplaceShortcutItem(Character character, int barNumber, int slotNumber, ShortcutItem shortcutItem);
        ShortcutBar GetShortcutBar(Character character, int barNumber);

        // SkillTreeItem
        bool InsertSkillTreeItem(SkillTreeItem skillTreeItem);
        SkillTreeItem SelectSkillTreeItemById(int id);
        List<SkillTreeItem> SelectSkillTreeItemsByCharId(int charId);
        SkillTreeItem SelectSkillTreeItemByCharSkillId(int charId, int skillId);
        bool UpdateSkillTreeItem(SkillTreeItem skillTreeItem);
        bool DeleteSkillTreeItem(int id);

        // Monster Coord
        bool InsertMonsterCoords(MonsterCoord monsterCoord);
        List<MonsterCoord> SelectMonsterCoords();
        List<MonsterCoord> SelectMonsterCoordsById(int id);
        List<MonsterCoord> SelectMonsterCoordsByMonsterId(int monsterId);
        List<MonsterCoord> SelectMonsterCoordsByMapId(int mapId);
        bool UpdateMonsterCoord(MonsterCoord monsterCoord);
        bool DeleteMonsterCoord(int monsterSpawnId);

        // Quest
        bool InsertQuest(Quest quest);
        Quest SelectQuestById(int questId);
        bool UpdateQuest(Quest quest);
        bool DeleteQuest(int questId);

        //Union
        bool InsertUnion(Union union);
        Union SelectUnionById(int unionId);
        Union SelectUnionByLeaderId(int leaderId);
        Union SelectUnionByName(string unionName);
        bool UpdateUnion(Union union);
        bool DeleteUnion(int unionId);

        //UnionMember
        bool InsertUnionMember(UnionMember unionMember);
        UnionMember SelectUnionMemberByCharacterId(int characterDatabaseId);
        List<UnionMember> SelectUnionMembersByUnionId(int unionId);
        bool UpdateUnionMember(UnionMember unionMember);
        bool DeleteUnionMember(int characterDatabaseId);
        bool DeleteAllUnionMembers(int unionId);

        // Gimmick Spawn
        bool InsertGimmick(Gimmick gimmick);
        List<Gimmick> SelectGimmicks();
        List<Gimmick> SelectGimmicksByMapId(int mapId);
        bool UpdateGimmick(Gimmick gimmick);
        bool DeleteGimmick(int gimmickId);

        // MapTransition Spawn
        bool InsertMapTransition(MapTransition mapTran);
        List<MapTransition> SelectMapTransitions();
        MapTransition SelectMapTransitionsById(int id);
        List<MapTransition> SelectMapTransitionsByMapId(int mapId);
        bool UpdateMapTransition(MapTransition mapTran);

        bool DeleteMapTransition(int mapTranId);

        // GGate Spawn
        bool InsertGGateSpawn(GGateSpawn gGateSpawn);
        List<GGateSpawn> SelectGGateSpawns();
        List<GGateSpawn> SelectGGateSpawnsByMapId(int mapId);
        bool UpdateGGateSpawn(GGateSpawn gGateSpawn);
        bool DeleteGGateSpawn(int gGateSpawnId);
    }
}
