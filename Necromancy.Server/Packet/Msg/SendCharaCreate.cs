using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Stats;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Msg
{
    public class SendCharaCreate : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendCharaCreate));
        private readonly int[] _fighterItems = {10300199, 15000199, 100101, 200109, 310101, 410101, 510101};
        private readonly int[] _fighterSkills = {11101, 11201};
        private readonly int[] _mageItems = {11300199, 120110, 220101, 320101, 420101, 520101};
        private readonly int[] _mageSkills = {13101, 13404};
        private readonly int[] _priestItems = {11000199, 15000199, 120111, 220101, 320101, 420101, 520101};
        private readonly int[] _priestSkills = {12501, 12601};

        // ToDo should we have separate claases for each class?  Fighter, Mage, Priest and Thief
        private readonly int[] _thiefItems = {10200199, 15000199, 110101, 200101, 300101, 400101, 500101};

        // ToDo should we have separate claases for each class?  Fighter, Mage, Priest and Thief
        private readonly int[] _thiefSkills = {14101, 14302, 14803};

        public SendCharaCreate(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_chara_create;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte characterSlotId = packet.data.ReadByte();
            string characterName = packet.data.ReadCString();
            uint raceId = packet.data.ReadUInt32();
            uint sexId = packet.data.ReadUInt32();

            byte hairId = packet.data.ReadByte();
            byte hairColorId = packet.data.ReadByte();
            byte faceId = packet.data.ReadByte();
            byte faceArrangeId = packet.data.ReadByte();
            byte voiceId = packet.data.ReadByte();

            uint alignmentId = packet.data.ReadUInt32();

            ushort strength = packet.data.ReadUInt16(); //bonus stat, not base
            ushort vitality = packet.data.ReadUInt16(); //bonus stat, not base
            ushort dexterity = packet.data.ReadUInt16(); //bonus stat, not base
            ushort agility = packet.data.ReadUInt16(); //bonus stat, not base
            ushort intelligence = packet.data.ReadUInt16(); //bonus stat, not base
            ushort piety = packet.data.ReadUInt16(); //bonus stat, not base
            ushort luck = packet.data.ReadUInt16(); //bonus stat, not base

            uint classId = packet.data.ReadUInt32();
            int errorCheck = packet.data.ReadInt32();
            byte unknown = packet.data.ReadByte();


            //-------------------------------------
            // Send Character Creation packets to Database for laster use.

            if (!maps.TryGet(Map.NEW_CHARACTER_MAP_ID, out Map map))
            {
                _Logger.Error($"New character map not found MapId: {Map.NEW_CHARACTER_MAP_ID}");
                client.Close();
            }

            Character character = new Character();
            Attribute attribute = new Attribute();
            attribute.DefaultClassAtributes(raceId);

            character.mapId = map.id;
            character.x = map.x;
            character.y = map.y;
            character.z = map.z;
            character.heading = map.orientation;

            character.accountId = client.account.id;
            character.soulId = client.soul.id;
            character.slot = characterSlotId;
            character.name = characterName;

            character.raceId = raceId;
            character.sexId = sexId;
            character.hairId = hairId;
            character.hairColorId = hairColorId;
            character.faceId = faceId;
            character.faceArrangeId = faceArrangeId;
            character.voiceId = voiceId;
            character.Hp.SetMax(attribute.hp);
            character.Hp.SetCurrent(attribute.hp);
            character.Mp.SetMax(attribute.mp);
            character.Mp.SetCurrent(attribute.mp);
            character.strength = (ushort)(strength + attribute.str);
            character.vitality = (ushort)(vitality + attribute.vit);
            character.dexterity = (ushort)(dexterity + attribute.dex);
            character.agility = (ushort)(agility + attribute.agi);
            character.intelligence = (ushort)(intelligence + attribute.@int);
            character.piety = (ushort)(piety + attribute.pie);
            character.luck = (ushort)(luck + attribute.luck);
            character.classId = classId;
            character.level = 1;

            //----------------------------------------------------------
            // Character Slot ID

            if (!database.InsertCharacter(character))
            {
                _Logger.Error(client, $"Failed to create CharacterSlot: {characterSlotId}");
                client.Close();
                return;
            }

            //after the DB instert, so Character has a valid ID.
            server.instances.AssignInstance(character);
            client.character = character;


            CreateSkillTreeItems(client, character, classId);
            CreateShortcutBars(client, character, classId);
            CreateEquipmentItems(client, character, classId);

            _Logger.Info($"Created CharacterSlot: {characterSlotId}");
            _Logger.Info($"Created CharacterName: {characterName}");
            _Logger.Info($"Created race: {raceId}");
            _Logger.Info($"Created sex: {sexId}");
            _Logger.Info($"Created hair: {hairId}");
            _Logger.Info($"Created hair color: {hairColorId}");
            _Logger.Info($"Created faceid: {faceId}");
            _Logger.Info($"Created alignment_id: {alignmentId}");
            _Logger.Info($"Created strength: {strength}");
            _Logger.Info($"Created vitality: {vitality}");
            _Logger.Info($"Created dexterity: {dexterity}");
            _Logger.Info($"Created agility: {agility}");
            _Logger.Info($"Created intelligence: {intelligence}");
            _Logger.Info($"Created piety: {piety}");
            _Logger.Info($"Created luck: {luck}");
            _Logger.Info($"Created class_id: {classId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(errorCheck);
            res.WriteInt32(character.id); //CharacterId

            router.Send(client, (ushort)MsgPacketId.recv_chara_create_r, res, ServerType.Msg);
        }

        private void CreateSkillTreeItems(NecClient client, Character character, uint classId)
        {
            if (classId == 0) // Fighter
                for (int i = 0; i < _fighterSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.level = 1;
                    skillTreeItem.skillId = _fighterSkills[i];
                    skillTreeItem.charId = character.id;
                    if (!database.InsertSkillTreeItem(skillTreeItem))
                    {
                        _Logger.Error(client, "Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            else if (classId == 1) // Thief
                for (int i = 0; i < _thiefSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.level = 1;
                    skillTreeItem.skillId = _thiefSkills[i];
                    skillTreeItem.charId = character.id;
                    if (!database.InsertSkillTreeItem(skillTreeItem))
                    {
                        _Logger.Error(client, "Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            else if (classId == 2) // Mage
                for (int i = 0; i < _mageSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.level = 1;
                    skillTreeItem.skillId = _mageSkills[i];
                    skillTreeItem.charId = character.id;
                    if (!database.InsertSkillTreeItem(skillTreeItem))
                    {
                        _Logger.Error(client, "Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            else if (classId == 3) // Priest
                for (int i = 0; i < _priestSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.level = 1;
                    skillTreeItem.skillId = _priestSkills[i];
                    skillTreeItem.charId = character.id;
                    if (!database.InsertSkillTreeItem(skillTreeItem))
                    {
                        _Logger.Error(client, "Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
        }

        private void CreateShortcutBars(NecClient client, Character character, uint classId)
        {
            if (classId == 0) // Fighter
            {
                //TODO Fix magic numbers all over the place
                database.InsertOrReplaceShortcutItem(character, 0, 0, new ShortcutItem(11101, ShortcutItem.ShortcutType.Skill));
                database.InsertOrReplaceShortcutItem(character, 0, 1, new ShortcutItem(11201, ShortcutItem.ShortcutType.Skill));
            }
            else if (classId == 1) // Thief
            {
                database.InsertOrReplaceShortcutItem(character, 0, 0, new ShortcutItem(14101, ShortcutItem.ShortcutType.Skill));
                database.InsertOrReplaceShortcutItem(character, 0, 1, new ShortcutItem(14302, ShortcutItem.ShortcutType.Skill));
                database.InsertOrReplaceShortcutItem(character, 0, 2, new ShortcutItem(14803, ShortcutItem.ShortcutType.Skill));
            }
            else if (classId == 2) // Mage
            {
                database.InsertOrReplaceShortcutItem(character, 0, 0, new ShortcutItem(13101, ShortcutItem.ShortcutType.Skill));
                database.InsertOrReplaceShortcutItem(character, 0, 1, new ShortcutItem(13404, ShortcutItem.ShortcutType.Skill));
            }
            else if (classId == 3) // Priest
            {
                database.InsertOrReplaceShortcutItem(character, 0, 0, new ShortcutItem(12501, ShortcutItem.ShortcutType.Skill));
                database.InsertOrReplaceShortcutItem(character, 0, 1, new ShortcutItem(12601, ShortcutItem.ShortcutType.Skill));
            }

            database.InsertOrReplaceShortcutItem(character, 0, 4, new ShortcutItem(11, ShortcutItem.ShortcutType.System));
            database.InsertOrReplaceShortcutItem(character, 0, 6, new ShortcutItem(18, ShortcutItem.ShortcutType.System));
            database.InsertOrReplaceShortcutItem(character, 0, 7, new ShortcutItem(22, ShortcutItem.ShortcutType.System));
            database.InsertOrReplaceShortcutItem(character, 0, 9, new ShortcutItem(2, ShortcutItem.ShortcutType.System));


            ShortcutBar shortcutBar1 = new ShortcutBar();
            database.InsertOrReplaceShortcutItem(character, 1, 0, new ShortcutItem(1, ShortcutItem.ShortcutType.Emote));
            database.InsertOrReplaceShortcutItem(character, 1, 1, new ShortcutItem(2, ShortcutItem.ShortcutType.Emote));
            database.InsertOrReplaceShortcutItem(character, 1, 2, new ShortcutItem(4, ShortcutItem.ShortcutType.Emote));
            database.InsertOrReplaceShortcutItem(character, 1, 3, new ShortcutItem(5, ShortcutItem.ShortcutType.Emote));
            database.InsertOrReplaceShortcutItem(character, 1, 4, new ShortcutItem(6, ShortcutItem.ShortcutType.Emote));
            database.InsertOrReplaceShortcutItem(character, 1, 5, new ShortcutItem(7, ShortcutItem.ShortcutType.Emote));
            database.InsertOrReplaceShortcutItem(character, 1, 6, new ShortcutItem(11, ShortcutItem.ShortcutType.Emote));
            database.InsertOrReplaceShortcutItem(character, 1, 7, new ShortcutItem(14, ShortcutItem.ShortcutType.Emote));
            database.InsertOrReplaceShortcutItem(character, 1, 8, new ShortcutItem(15, ShortcutItem.ShortcutType.Emote));
            database.InsertOrReplaceShortcutItem(character, 1, 9, new ShortcutItem(16, ShortcutItem.ShortcutType.Emote));
        }

        private void CreateEquipmentItems(NecClient client, Character character, uint classId)
        {
            if (classId == 0) // Fighter
                SendItems(client, _fighterItems);
            else if (classId == 1) // Thief
                SendItems(client, _thiefItems);
            else if (classId == 2) // Mage
                SendItems(client, _mageItems);
            else if (classId == 3) // Priest
                SendItems(client, _priestItems);
        }


        public void SendItems(NecClient client, int[] itemIds)
        {
            ItemSpawnParams[] spawmParams = new ItemSpawnParams[itemIds.Length];
            for (int i = 0; i < itemIds.Length; i++)
            {
                spawmParams[i] = new ItemSpawnParams();
                spawmParams[i].itemStatuses = ItemStatuses.Identified;
            }

            ItemService itemService = new ItemService(client.character);
            itemService.SpawnItemInstances(ItemZoneType.AdventureBag, itemIds, spawmParams);
        }
    }
}
