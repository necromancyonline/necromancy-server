using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Stats;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_create : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_chara_create));

        public send_chara_create(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)MsgPacketId.send_chara_create;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte character_slot_id = packet.Data.ReadByte();
            string character_name = packet.Data.ReadCString();
            uint race_id = packet.Data.ReadUInt32();
            uint sex_id = packet.Data.ReadUInt32();

            byte hair_id = packet.Data.ReadByte();
            byte hair_color_id = packet.Data.ReadByte();
            byte face_id = packet.Data.ReadByte();
            byte face_arrange_id = packet.Data.ReadByte();
            byte voice_id = packet.Data.ReadByte();

            uint alignment_id = packet.Data.ReadUInt32();

            ushort strength = packet.Data.ReadUInt16(); //bonus stat, not base
            ushort vitality = packet.Data.ReadUInt16();//bonus stat, not base
            ushort dexterity = packet.Data.ReadUInt16();//bonus stat, not base
            ushort agility = packet.Data.ReadUInt16();//bonus stat, not base
            ushort intelligence = packet.Data.ReadUInt16();//bonus stat, not base
            ushort piety = packet.Data.ReadUInt16();//bonus stat, not base
            ushort luck = packet.Data.ReadUInt16();//bonus stat, not base

            uint class_id = packet.Data.ReadUInt32();
            int error_check = packet.Data.ReadInt32();
            byte unknown = packet.Data.ReadByte();


            //-------------------------------------
            // Send Character Creation packets to Database for laster use.

            if (!Maps.TryGet(Map.NewCharacterMapId, out Map map))
            {
                Logger.Error($"New character map not found MapId: {Map.NewCharacterMapId}");
                client.Close();
            }

            Character character = new Character();
            Attribute attribute = new Attribute();
            attribute.DefaultClassAtributes(race_id);

            character.MapId = map.Id;
            character.X = map.X;
            character.Y = map.Y;
            character.Z = map.Z;
            character.Heading = (byte)map.Orientation;

            character.AccountId = client.Account.Id;
            character.SoulId = client.Soul.Id;
            character.Slot = character_slot_id;
            character.Name = character_name;

            character.RaceId = race_id;
            character.SexId = sex_id;
            character.HairId = hair_id;
            character.HairColorId = hair_color_id;
            character.FaceId = face_id;
            character.FaceArrangeId = face_arrange_id;
            character.VoiceId = voice_id;
            character.Hp.setMax(attribute.Hp);
            character.Hp.setCurrent(attribute.Hp);
            character.Mp.setMax(attribute.Mp);
            character.Mp.setCurrent(attribute.Mp);
            character.Strength = (ushort)(strength + attribute.Str);
            character.Vitality = (ushort)(vitality + attribute.Vit);
            character.Dexterity = (ushort)(dexterity + attribute.Dex);
            character.Agility = (ushort)(agility + attribute.Agi);
            character.Intelligence = (ushort)(intelligence + attribute.Int);
            character.Piety = (ushort)(piety + attribute.Pie);
            character.Luck = (ushort)(luck + attribute.Luck);
            character.ClassId = class_id;
            character.Level = 1;

            //----------------------------------------------------------
            // Character Slot ID

            if (!Database.InsertCharacter(character))
            {
                Logger.Error(client, $"Failed to create CharacterSlot: {character_slot_id}");
                client.Close();
                return;
            }
            //after the DB instert, so Character has a valid ID.
            Server.Instances.AssignInstance(character);
            client.Character = character;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       

            CreateSkillTreeItems(client, character, class_id);
            CreateShortcutBars(client, character, class_id);
            CreateEquipmentItems(client, character, class_id);

            Logger.Info($"Created CharacterSlot: {character_slot_id}");
            Logger.Info($"Created CharacterName: {character_name}");
            Logger.Info($"Created race: {race_id}");
            Logger.Info($"Created sex: {sex_id}");
            Logger.Info($"Created hair: {hair_id}");
            Logger.Info($"Created hair color: {hair_color_id}");
            Logger.Info($"Created faceid: {face_id}");
            Logger.Info($"Created alignment_id: {alignment_id}");
            Logger.Info($"Created strength: {strength}");
            Logger.Info($"Created vitality: {vitality}");
            Logger.Info($"Created dexterity: {dexterity}");
            Logger.Info($"Created agility: {agility}");
            Logger.Info($"Created intelligence: {intelligence}");
            Logger.Info($"Created piety: {piety}");
            Logger.Info($"Created luck: {luck}");
            Logger.Info($"Created class_id: {class_id}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(error_check);
            res.WriteInt32(character.Id); //CharacterId

            Router.Send(client, (ushort)MsgPacketId.recv_chara_create_r, res, ServerType.Msg);
        }

        private void CreateSkillTreeItems(NecClient client, Character character, uint class_id)
        {
            if (class_id == 0) // Fighter
            {
                for (int i = 0; i < fighterSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.Level = 1;
                    skillTreeItem.SkillId = fighterSkills[i];
                    skillTreeItem.CharId = character.Id;
                    if (!Database.InsertSkillTreeItem(skillTreeItem))
                    {
                        Logger.Error(client, $"Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            }
            else if (class_id == 1) // Thief
            {
                for (int i = 0; i < thiefSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.Level = 1;
                    skillTreeItem.SkillId = thiefSkills[i];
                    skillTreeItem.CharId = character.Id;
                    if (!Database.InsertSkillTreeItem(skillTreeItem))
                    {
                        Logger.Error(client, $"Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            }
            else if (class_id == 2) // Mage
            {
                for (int i = 0; i < mageSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.Level = 1;
                    skillTreeItem.SkillId = mageSkills[i];
                    skillTreeItem.CharId = character.Id;
                    if (!Database.InsertSkillTreeItem(skillTreeItem))
                    {
                        Logger.Error(client, $"Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            }
            else if (class_id == 3) // Priest
            {
                for (int i = 0; i < priestSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.Level = 1;
                    skillTreeItem.SkillId = priestSkills[i];
                    skillTreeItem.CharId = character.Id;
                    if (!Database.InsertSkillTreeItem(skillTreeItem))
                    {
                        Logger.Error(client, $"Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            }
        }

        // ToDo should we have separate claases for each class?  Fighter, Mage, Priest and Thief
        int[] thiefSkills = new int[] { 14101, 14302, 14803 };
        int[] fighterSkills = new int[] { 11101, 11201 };
        int[] mageSkills = new int[] { 13101, 13404 };
        int[] priestSkills = new int[] { 12501, 12601 };

        private void CreateShortcutBars(NecClient client, Character character, uint class_id)
        {
            if (class_id == 0) // Fighter
            {
                //TODO Fix magic numbers all over the place
                Database.InsertOrReplaceShortcutItem(character, 0, 0, new ShortcutItem(11101, ShortcutItem.ShortcutType.SKILL));
                Database.InsertOrReplaceShortcutItem(character, 0, 1, new ShortcutItem(11201, ShortcutItem.ShortcutType.SKILL));
            }
            else if (class_id == 1) // Thief
            {
                Database.InsertOrReplaceShortcutItem(character, 0, 0, new ShortcutItem(14101, ShortcutItem.ShortcutType.SKILL));
                Database.InsertOrReplaceShortcutItem(character, 0, 1, new ShortcutItem(14302, ShortcutItem.ShortcutType.SKILL));
                Database.InsertOrReplaceShortcutItem(character, 0, 2, new ShortcutItem(14803, ShortcutItem.ShortcutType.SKILL));
            }
            else if (class_id == 2) // Mage
            {
                Database.InsertOrReplaceShortcutItem(character, 0, 0, new ShortcutItem(13101, ShortcutItem.ShortcutType.SKILL));
                Database.InsertOrReplaceShortcutItem(character, 0, 1, new ShortcutItem(13404, ShortcutItem.ShortcutType.SKILL));
            }
            else if (class_id == 3) // Priest
            {
                Database.InsertOrReplaceShortcutItem(character, 0, 0, new ShortcutItem(12501, ShortcutItem.ShortcutType.SKILL));
                Database.InsertOrReplaceShortcutItem(character, 0, 1, new ShortcutItem(12601, ShortcutItem.ShortcutType.SKILL));
            }

            Database.InsertOrReplaceShortcutItem(character, 0, 4, new ShortcutItem(11, ShortcutItem.ShortcutType.SYSTEM));
            Database.InsertOrReplaceShortcutItem(character, 0, 6, new ShortcutItem(18, ShortcutItem.ShortcutType.SYSTEM));
            Database.InsertOrReplaceShortcutItem(character, 0, 7, new ShortcutItem(22, ShortcutItem.ShortcutType.SYSTEM));
            Database.InsertOrReplaceShortcutItem(character, 0, 9, new ShortcutItem(2, ShortcutItem.ShortcutType.SYSTEM));


            ShortcutBar shortcutBar1 = new ShortcutBar();
            Database.InsertOrReplaceShortcutItem(character, 1, 0, new ShortcutItem(1, ShortcutItem.ShortcutType.EMOTE));
            Database.InsertOrReplaceShortcutItem(character, 1, 1, new ShortcutItem(2, ShortcutItem.ShortcutType.EMOTE));
            Database.InsertOrReplaceShortcutItem(character, 1, 2, new ShortcutItem(4, ShortcutItem.ShortcutType.EMOTE));
            Database.InsertOrReplaceShortcutItem(character, 1, 3, new ShortcutItem(5, ShortcutItem.ShortcutType.EMOTE));
            Database.InsertOrReplaceShortcutItem(character, 1, 4, new ShortcutItem(6, ShortcutItem.ShortcutType.EMOTE));
            Database.InsertOrReplaceShortcutItem(character, 1, 5, new ShortcutItem(7, ShortcutItem.ShortcutType.EMOTE));
            Database.InsertOrReplaceShortcutItem(character, 1, 6, new ShortcutItem(11, ShortcutItem.ShortcutType.EMOTE));
            Database.InsertOrReplaceShortcutItem(character, 1, 7, new ShortcutItem(14, ShortcutItem.ShortcutType.EMOTE));
            Database.InsertOrReplaceShortcutItem(character, 1, 8, new ShortcutItem(15, ShortcutItem.ShortcutType.EMOTE));
            Database.InsertOrReplaceShortcutItem(character, 1, 9, new ShortcutItem(16, ShortcutItem.ShortcutType.EMOTE));
        }

        private void CreateEquipmentItems(NecClient client, Character character, uint class_id)
        {
            if (class_id == 0) // Fighter
            {
                SendItems(client, fighterItems);
            }
            else if (class_id == 1) // Thief
            {
                SendItems(client, thiefItems);
            }
            else if (class_id == 2) // Mage
            {
                SendItems(client, mageItems);
            }
            else if (class_id == 3) // Priest
            {
                SendItems(client, priestItems);
            }
        }

        // ToDo should we have separate claases for each class?  Fighter, Mage, Priest and Thief
        int[] thiefItems = new int[]    { 10200199, 15000199, 110101, 200101, 300101, 400101, 500101};
        int[] fighterItems = new int[]  { 10300199, 15000199, 100101, 200109, 310101, 410101, 510101};
        int[] mageItems = new int[]     { 11300199,           120110, 220101, 320101, 420101, 520101};
        int[] priestItems = new int[]   { 11000199, 15000199, 120111, 220101, 320101, 420101, 520101};


        public void SendItems(NecClient client, int[] itemIds)
        {
            ItemSpawnParams[] spawmParams = new ItemSpawnParams[itemIds.Length];
            for (int i = 0; i < itemIds.Length; i++)
            {
                spawmParams[i] = new ItemSpawnParams();
                spawmParams[i].ItemStatuses = ItemStatuses.Identified;
            }
            ItemService itemService = new ItemService(client.Character);
            itemService.SpawnItemInstances(ItemZoneType.AdventureBag, itemIds, spawmParams);
        }

    }
}
