using Arrowgene.Logging;
using Necromancy.Server.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace Necromancy.Server.Systems.Item
{
    class ItemDao : DatabaseAccessObject, IItemDao
    {    

        private const string SqlSelectItemInstanceById = @"
            SELECT
                *
            FROM
                nec_item_instance
            WHERE
                id = @id";

        private const string SqlSelectItemInstanceByLocation = @"
            SELECT
                *
            FROM
                nec_item_instance
            WHERE
                character_id = @character_id
            AND
                zone = @zone
            AND
                bag = @bag
            AND
                slot = @slot";

        private const string SqlSelectSpawnedItemByIds = @"
            SELECT
                *
            FROM
                nec_item_instance
            WHERE
                id IN ({0})";

        private const string SqlSelectOwnedInventoryItems = @"
            SELECT 
                * 
            FROM 
                item_instance 
            WHERE 
                owner_id = @owner_id 
            AND 
                zone IN (0,1,2,8,12)"; //adventure bag, equipped bags,royal bag, bag slot, avatar inventory

        private const string SqlSelectLootableInventoryItems = @"
            SELECT 
                * 
            FROM 
                item_instance 
            WHERE 
                owner_id = @owner_id 
            AND 
                zone IN (0,1,2)"; //adventure bag, equipped bags,royal bag

        private const string SqlUpdateItemLocation = @"
            UPDATE 
                nec_item_instance 
            SET 
                zone = @zone, container = @container, slot = @slot 
            WHERE 
                id = @id";

        private const string SqlUpdateItemQuantity = @"
            UPDATE 
                nec_item_instance 
            SET 
                quantity = @quantity 
            WHERE 
                id = @id";

        private const string SqlUpdateItemEquipMask = @"
            UPDATE 
                nec_item_instance 
            SET 
                current_equip_slot = @current_equip_slot 
            WHERE 
                id = @id";

        private const string SqlUpdateItemEnhancementLevel = @"
            UPDATE 
                nec_item_instance 
            SET 
                enhancement_level = @enhancement_level 
            WHERE 
                id = @id";

        private const string SqlUpdateItemCurrentDurability = @"
            UPDATE 
                nec_item_instance 
            SET 
                current_durability = @current_durability 
            WHERE 
                id = @id";

        private const string SqlUpdateItemOwnerAndStatus = @"
            UPDATE 
                nec_item_instance 
            SET 
                owner_id = @owner_id, statuses = @statuses 
            WHERE 
                id = @id";

        private const string SqlDeleteItemInstance = @"
            DELETE FROM 
                nec_item_instance 
            WHERE 
                id = @id";

        private const string SqlInsertItemInstances = @"
            INSERT INTO 
	            nec_item_instance
		        (
			        owner_id,
			        zone,
			        container,
			        slot,
			        base_id,
                    statuses,
                    quantity,
                    gem_slot_1_type,
                    gem_slot_2_type,
                    gem_slot_3_type,
                    gem_id_slot_1,
                    gem_id_slot_2,
                    gem_id_slot_3,
                    plus_maximum_durability,
                    plus_physical,
                    plus_magical,
                    plus_gp,
                    plus_weight,
                    plus_ranged_eff,
                    plus_reservoir_eff
		        )		
            VALUES
	            (
                    @owner_id,
                    @zone,
                    @container,
                    @slot,
                    @base_id,
                    @statuses,
                    @quantity,
                    @gem_slot_1_type,
                    @gem_slot_2_type,
                    @gem_slot_3_type,
                    @gem_id_slot_1,
                    @gem_id_slot_2,
                    @gem_id_slot_3,
                    @plus_maximum_durability,
                    @plus_physical,
                    @plus_magical,
                    @plus_gp,
                    @plus_weight,
                    @plus_ranged_eff,
                    @plus_reservoir_eff
                );
            SELECT last_insert_rowid()";

        private const string SqlSelectAuctions = @"
            SELECT 			
                item_instance.*,
				nec_soul.id AS owner_soul_id
            FROM 
                item_instance
			JOIN
				nec_soul
			ON
				item_instance.owner_id = nec_soul.id			
            WHERE                 
                zone = 82
            AND
                owner_soul_id != @owner_soul_id";

        private const string SqlUpdateExhibit = @"
            UPDATE 
                nec_item_instance 
            SET 
                consigner_soul_name = @consigner_soul_name, expiry_datetime = @expiry_datetime, min_bid = @min_bid, buyout_price = @buyout_price, comment = @comment 
            WHERE 
                id = @id";

        private const string SqlUpdateCancelExhibit = @"
            UPDATE 
                nec_item_instance 
            SET 
                consigner_soul_name = @consigner_soul_name, expiry_datetime = @expiry_datetime, min_bid = @min_bid, buyout_price = @buyout_price, comment = @comment
            WHERE 
                id = @id";

        private const string SqlSelectBids = @"
            SELECT 
                item_instance.*, 
                bidder_soul_id, 
                current_bid, 
                (SELECT MAX(current_bid) FROM nec_auction_bids WHERE item_instance_id = id) AS max_bid
            FROM 
                item_instance 
            JOIN 
                nec_auction_bids 
            ON 
                item_instance.id = nec_auction_bids.item_instance_id 
            WHERE 
                bidder_soul_id = @bidder_soul_id";

        private const string SqlSelectLots = @"
            SELECT 			
                item_instance.*,
				nec_soul.id AS owner_soul_id
            FROM 
                item_instance
			JOIN
				nec_soul
			ON
				item_instance.owner_id = nec_soul.id
            WHERE 
                owner_soul_id = @owner_soul_id
            AND 
                zone = 82"; //Probably auction lot zone, may be 83

        public ItemInstance InsertItemInstance(int baseId)
        {
            throw new NotImplementedException();
        }

        public ItemInstance SelectItemInstance(long instanceId)
        {
            ItemInstance itemInstance = null;
            ExecuteReader(SqlSelectItemInstanceById,
                command =>
                {
                    AddParameter(command, "@id", instanceId);
                }, reader =>
                {
                    itemInstance = MakeItemInstance(reader);
                });
            return itemInstance;
        }

        public ItemInstance[] SelectItemInstances(long[] instanceIds)
        {
            throw new NotImplementedException();
        }
        public void DeleteItemInstance(ulong instanceId)
        {
            ExecuteNonQuery(SqlDeleteItemInstance,
                command =>
                {
                    AddParameter(command, "@id", instanceId);
                });
        }

        public void UpdateItemEquipMask(ulong instanceId, ItemEquipSlots equipSlots)
        {
            ExecuteNonQuery(SqlUpdateItemEquipMask,
                command =>
                {
                    AddParameter(command, "@current_equip_slot", (int)equipSlots);
                    AddParameter(command, "@id", instanceId);
                });
        }

        public void UpdateItemEnhancementLevel(ulong instanceId, int level)
        {
            ExecuteNonQuery(SqlUpdateItemEnhancementLevel,
                command =>
                {
                    AddParameter(command, "@enhancement_level", level);
                    AddParameter(command, "@id", instanceId);
                });
        }

        public void UpdateItemOwnerAndStatus(ulong instanceId, int ownerId, int statuses)
        {
            ExecuteNonQuery(SqlUpdateItemOwnerAndStatus,
                command =>
                {
                    AddParameter(command, "@statuses", statuses);
                    AddParameter(command, "@owner_id", ownerId);
                    AddParameter(command, "@id", instanceId);
                });
        }
        public void UpdateItemCurrentDurability(ulong instanceId, int currentDurability)
        {
            ExecuteNonQuery(SqlUpdateItemCurrentDurability,
                command =>
                {
                    AddParameter(command, "@current_durability", currentDurability);
                    AddParameter(command, "@id", instanceId);
                });
        }

        public ItemInstance SelectItemInstance(int characterId, ItemLocation itemLocation)
        {
            throw new NotImplementedException();
        }

        public void UpdateItemLocation(ulong instanceId, ItemLocation loc)
        {
            ulong[] instanceIds = new ulong[1] { instanceId };
            ItemLocation[] locs = new ItemLocation[1] { loc };
            UpdateItemLocations(instanceIds, locs);
        }

        public void UpdateItemLocations(ulong[] instanceIds, ItemLocation[] locs)
        {
            int size = instanceIds.Length;
            try
            {
                using DbConnection conn = GetSQLConnection();
                conn.Open();
                using DbCommand command = conn.CreateCommand();
                command.CommandText = SqlUpdateItemLocation;
                for (int i = 0; i < size; i++)
                {
                    command.Parameters.Clear();
                    AddParameter(command, "@zone", (byte)locs[i].ZoneType);
                    AddParameter(command, "@container", locs[i].Container);
                    AddParameter(command, "@slot", locs[i].Slot);
                    AddParameter(command, "@id", instanceIds[i]);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Query: {SqlUpdateItemLocation}");
                Exception(ex);
            }
        }

        public void UpdateItemQuantities(ulong[] instanceIds, byte[] quantities)
        {
            int size = instanceIds.Length;
            try
            {
                using DbConnection conn = GetSQLConnection();
                conn.Open();
                using DbCommand command = conn.CreateCommand();
                command.CommandText = SqlUpdateItemQuantity;
                for (int i = 0; i < size; i++)
                {
                    command.Parameters.Clear();
                    AddParameter(command, "@quantity", quantities[i]);
                    AddParameter(command, "@id", instanceIds[i]);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Query: {SqlUpdateItemQuantity}");
                Exception(ex);
            }
        }

        /// <summary>
        /// This selects only the items in the player's inventory: Adventure bag, Equipped bags, Royal bag, Bag Slots, and Avatar inventory.
        /// </summary>
        /// <param name="ownerId">Owner of items.</param>
        /// <returns></returns>
        public List<ItemInstance> SelectOwnedInventoryItems(int ownerId)
        {
            List<ItemInstance> ownedItemInstances = new List<ItemInstance>();
            ExecuteReader(SqlSelectOwnedInventoryItems,
                command =>
                {
                    AddParameter(command, "@owner_id", ownerId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        ownedItemInstances.Add(MakeItemInstance(reader));
                    }
                });
            return ownedItemInstances;
        }

        public List<ItemInstance> InsertItemInstances(int ownerId, ItemLocation[] locs, int[] baseId, ItemSpawnParams[] spawnParams)
        {
            int size = locs.Length;
            List<ItemInstance> itemInstances = new List<ItemInstance>(size);
            try
            {
                using DbConnection conn = GetSQLConnection();
                conn.Open();
                using DbCommand command = conn.CreateCommand();
                command.CommandText = SqlInsertItemInstances;
                long[] lastIds = new long[size];
                for (int i = 0; i < size; i++)
                {
                    command.Parameters.Clear();
                    AddParameter(command, "@owner_id", ownerId);
                    AddParameter(command, "@zone", (byte)locs[i].ZoneType);
                    AddParameter(command, "@container", locs[i].Container);
                    AddParameter(command, "@slot", locs[i].Slot);
                    AddParameter(command, "@base_id", baseId[i]);
                    AddParameter(command, "@statuses", (int)spawnParams[i].ItemStatuses);
                    AddParameter(command, "@quantity", spawnParams[i].Quantity);
                    AddParameter(command, "@plus_maximum_durability", spawnParams[i].plus_maximum_durability);
                    AddParameter(command, "@plus_physical", spawnParams[i].plus_physical);
                    AddParameter(command, "@plus_magical", spawnParams[i].plus_magical);
                    AddParameter(command, "@plus_gp", spawnParams[i].plus_gp);
                    AddParameter(command, "@plus_weight", spawnParams[i].plus_weight);
                    AddParameter(command, "@plus_ranged_eff", spawnParams[i].plus_ranged_eff);
                    AddParameter(command, "@plus_reservoir_eff", spawnParams[i].plus_reservoir_eff);

                    if (spawnParams[i].GemSlots.Length > 0)
                        AddParameter(command, "@gem_slot_1_type", (int)spawnParams[i].GemSlots[0].Type);
                    else
                        AddParameter(command, "@gem_slot_1_type", 0);

                    if (spawnParams[i].GemSlots.Length > 1)
                        AddParameter(command, "@gem_slot_2_type", (int)spawnParams[i].GemSlots[1].Type);
                    else
                        AddParameter(command, "@gem_slot_2_type", 0);

                    if (spawnParams[i].GemSlots.Length > 2)
                        AddParameter(command, "@gem_slot_3_type", (int)spawnParams[i].GemSlots[2].Type);
                    else
                        AddParameter(command, "@gem_slot_3_type", 0);

                    if (spawnParams[i].GemSlots.Length > 0)
                        AddParameter(command, "@gem_id_slot_1", (int)spawnParams[i].GemSlots[0].Type);
                    else
                        AddParameterNull(command, "@gem_id_slot_1");

                    if (spawnParams[i].GemSlots.Length > 1)
                        AddParameter(command, "@gem_id_slot_2", (int)spawnParams[i].GemSlots[1].Type);
                    else
                        AddParameterNull(command, "@gem_id_slot_2");

                    if (spawnParams[i].GemSlots.Length > 2)
                        AddParameter(command, "@gem_id_slot_3", (int)spawnParams[i].GemSlots[2].Type);
                    else
                        AddParameterNull(command, "@gem_id_slot_3");

                    lastIds[i] = (long)command.ExecuteScalar();
                }

                string[] parameters = new string[size];
                for (int i = 0; i < size; i++)
                {
                    parameters[i] = string.Format("@id{0}", i);
                    AddParameter(command, parameters[i], lastIds[i]);
                }

                command.CommandText = string.Format("SELECT * FROM item_instance WHERE id IN({0})", string.Join(", ", parameters));
                using DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    itemInstances.Add(MakeItemInstance(reader));
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"Query: {SqlInsertItemInstances}");
                Exception(ex);
            }
            return itemInstances;
        }

        private ItemInstance MakeItemInstance(DbDataReader reader)
        {
            ulong instanceId = (ulong)reader.GetInt64("id");
            ItemInstance itemInstance = new ItemInstance(instanceId);
            itemInstance.OwnerID = reader.GetInt32("owner_id");

            ItemZoneType zone = (ItemZoneType)reader.GetByte("zone");
            byte bag = reader.GetByte("container");
            short slot = reader.GetInt16("slot");
            ItemLocation itemLocation = new ItemLocation(zone, bag, slot);
            itemInstance.Location = itemLocation;

            itemInstance.BaseID = reader.GetInt32("base_id");

            itemInstance.Quantity = reader.GetByte("quantity");

            itemInstance.Statuses = (ItemStatuses)reader.GetInt32("statuses");

            itemInstance.CurrentEquipSlot = (ItemEquipSlots)reader.GetInt32("current_equip_slot");

            itemInstance.CurrentDurability = reader.GetInt32("current_durability");
            itemInstance.MaximumDurability = reader.GetInt32("plus_maximum_durability");

            itemInstance.EnhancementLevel = reader.GetByte("enhancement_level");

            itemInstance.SpecialForgeLevel = reader.GetByte("special_forge_level");

            itemInstance.Physical = reader.GetInt16("physical");
            itemInstance.Magical = reader.GetInt16("magical");

            itemInstance.Hardness = reader.GetByte("hardness");
            itemInstance.PlusPhysical = reader.GetInt16("plus_physical");
            itemInstance.PlusMagical = reader.GetInt16("plus_magical");
            itemInstance.PlusGP = reader.GetInt16("plus_gp");
            itemInstance.PlusWeight = (short)(reader.GetInt16("plus_weight") * 10); //TODO REMOVE THIS MULTIPLICATION AND FIX TRHOUGHOUT CODE
            itemInstance.PlusRangedEff = reader.GetInt16("plus_ranged_eff");
            itemInstance.PlusReservoirEff = reader.GetInt16("plus_reservoir_eff");

            int gemSlotNum = 0;
            int gemSlot1Type = reader.GetByte("gem_slot_1_type");
            if (gemSlot1Type != 0) gemSlotNum++;
            int gemSlot2Type = reader.GetByte("gem_slot_2_type");
            if (gemSlot2Type != 0) gemSlotNum++;
            int gemSlot3Type = reader.GetByte("gem_slot_3_type");
            if (gemSlot3Type != 0) gemSlotNum++;
            GemSlot[] gemSlot = new GemSlot[gemSlotNum];

            itemInstance.EnchantId = reader.GetInt32("enchant_id");
            itemInstance.GP = reader.GetInt16("plus_gp");
            itemInstance.Type = (ItemType)Enum.Parse(typeof(ItemType), reader.GetString("item_type"));
            itemInstance.Quality = (ItemQualities)Enum.Parse(typeof(ItemQualities), reader.GetString("quality"), true);
            itemInstance.MaxStackSize = reader.GetByte("max_stack_size");

            if (reader.GetBoolean("es_hand_r")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.RightHand;
            if (reader.GetBoolean("es_hand_l")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.LeftHand;
            if (reader.GetBoolean("es_quiver")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Quiver;
            if (reader.GetBoolean("es_head")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Head;
            if (reader.GetBoolean("es_body")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Torso;
            if (reader.GetBoolean("es_legs")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Legs;
            if (reader.GetBoolean("es_arms")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Arms;
            if (reader.GetBoolean("es_feet")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Feet;
            if (reader.GetBoolean("es_mantle")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Cloak;
            if (reader.GetBoolean("es_ring")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Ring;
            if (reader.GetBoolean("es_earring")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Earring;
            if (reader.GetBoolean("es_necklace")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Necklace;
            if (reader.GetBoolean("es_belt")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Belt;
            if (reader.GetBoolean("es_talkring")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.Talkring;
            if (reader.GetBoolean("es_avatar_head")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.AvatarHead;
            if (reader.GetBoolean("es_avatar_body")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.AvatarTorso;
            if (reader.GetBoolean("es_avatar_legs")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.AvatarLegs;
            if (reader.GetBoolean("es_avatar_arms")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.AvatarArms;
            if (reader.GetBoolean("es_avatar_feet")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.AvatarFeet;
            if (reader.GetBoolean("es_avatar_feet")) itemInstance.EquipAllowedSlots |= ItemEquipSlots.AvatarCloak;

            if (reader.GetBoolean("req_hum_m")) itemInstance.RequiredRaces |= Races.HumanMale;
            if (reader.GetBoolean("req_hum_f")) itemInstance.RequiredRaces |= Races.HumanFemale;
            if (reader.GetBoolean("req_elf_m")) itemInstance.RequiredRaces |= Races.ElfMale;
            if (reader.GetBoolean("req_elf_f")) itemInstance.RequiredRaces |= Races.ElfFemale;
            if (reader.GetBoolean("req_dwf_m")) itemInstance.RequiredRaces |= Races.DwarfMale;
            if (reader.GetBoolean("req_por_m")) itemInstance.RequiredRaces |= Races.PorkulMale;
            if (reader.GetBoolean("req_por_f")) itemInstance.RequiredRaces |= Races.PorkulFemale;
            if (reader.GetBoolean("req_gnm_f")) itemInstance.RequiredRaces |= Races.GnomeFemale;

            if (reader.GetBoolean("req_fighter")) itemInstance.RequiredClasses |= Classes.Fighter;
            if (reader.GetBoolean("req_thief")) itemInstance.RequiredClasses |= Classes.Thief;
            if (reader.GetBoolean("req_mage")) itemInstance.RequiredClasses |= Classes.Mage;
            if (reader.GetBoolean("req_priest")) itemInstance.RequiredClasses |= Classes.Priest;
            if (reader.GetBoolean("req_samurai")) itemInstance.RequiredClasses |= Classes.Samurai;
            if (reader.GetBoolean("req_bishop")) itemInstance.RequiredClasses |= Classes.Bishop;
            if (reader.GetBoolean("req_ninja")) itemInstance.RequiredClasses |= Classes.Ninja;
            if (reader.GetBoolean("req_lord")) itemInstance.RequiredClasses |= Classes.Lord;
            if (reader.GetBoolean("req_clown")) itemInstance.RequiredClasses |= Classes.Clown;
            if (reader.GetBoolean("req_alchemist")) itemInstance.RequiredClasses |= Classes.Alchemist;

            if (reader.GetBoolean("req_lawful")) itemInstance.RequiredAlignments |= Alignments.Lawful;
            if (reader.GetBoolean("req_neutral")) itemInstance.RequiredAlignments |= Alignments.Neutral;
            if (reader.GetBoolean("req_chaotic")) itemInstance.RequiredAlignments |= Alignments.Chaotic;

            itemInstance.RequiredStrength = reader.GetByte("req_str");
            itemInstance.RequiredVitality = reader.GetByte("req_vit");
            itemInstance.RequiredDexterity = reader.GetByte("req_dex");
            itemInstance.RequiredAgility = reader.GetByte("req_agi");
            itemInstance.RequiredIntelligence = reader.GetByte("req_int");
            itemInstance.RequiredPiety = reader.GetByte("req_pie");
            itemInstance.RequiredLuck = reader.GetByte("req_luk");

            itemInstance.RequiredSoulRank = reader.GetByte("req_soul_rank");
            //todo max soul rank
            itemInstance.RequiredLevel = reader.GetByte("req_lvl");

            itemInstance.PhysicalSlash = reader.GetByte("phys_slash");
            itemInstance.PhysicalStrike = reader.GetByte("phys_strike");
            itemInstance.PhysicalPierce = reader.GetByte("phys_pierce");

            itemInstance.PhysicalDefenseFire = reader.GetByte("pdef_fire");
            itemInstance.PhysicalDefenseWater = reader.GetByte("pdef_water");
            itemInstance.PhysicalDefenseWind = reader.GetByte("pdef_wind");
            itemInstance.PhysicalDefenseEarth = reader.GetByte("pdef_earth");
            itemInstance.PhysicalDefenseLight = reader.GetByte("pdef_light");
            itemInstance.PhysicalDefenseDark = reader.GetByte("pdef_dark");

            itemInstance.MagicalAttackFire = reader.GetByte("matk_fire");
            itemInstance.MagicalAttackWater = reader.GetByte("matk_water");
            itemInstance.MagicalAttackWind = reader.GetByte("matk_wind");
            itemInstance.MagicalAttackEarth = reader.GetByte("matk_earth");
            itemInstance.MagicalAttackLight = reader.GetByte("matk_light");
            itemInstance.MagicalAttackDark = reader.GetByte("matk_dark");

            itemInstance.Hp = reader.GetByte("seffect_hp");
            itemInstance.Mp = reader.GetByte("seffect_mp");
            itemInstance.Str = reader.GetByte("seffect_str");
            itemInstance.Vit = reader.GetByte("seffect_vit");
            itemInstance.Dex = reader.GetByte("seffect_dex");
            itemInstance.Agi = reader.GetByte("seffect_agi");
            itemInstance.Int = reader.GetByte("seffect_int");
            itemInstance.Pie = reader.GetByte("seffect_pie");
            itemInstance.Luk = reader.GetByte("seffect_luk");

            itemInstance.ResistPoison = reader.GetByte("res_poison");
            itemInstance.ResistParalyze = reader.GetByte("res_paralyze");
            itemInstance.ResistPetrified = reader.GetByte("res_petrified");
            itemInstance.ResistFaint = reader.GetByte("res_faint");
            itemInstance.ResistBlind = reader.GetByte("res_blind");
            itemInstance.ResistSleep = reader.GetByte("res_sleep");
            itemInstance.ResistSilence = reader.GetByte("res_silent");
            itemInstance.ResistCharm = reader.GetByte("res_charm");
            itemInstance.ResistConfusion = reader.GetByte("res_confusion");
            itemInstance.ResistFear = reader.GetByte("res_fear");

            //itemInstance.StatusMalus = (ItemStatusEffect)Enum.Parse(typeof(ItemStatusEffect), reader.GetString("status_malus"));
            itemInstance.StatusMalusPercent = reader.GetInt32("status_percent");

            itemInstance.ObjectType = reader.GetString("object_type"); //not sure what this is for
            itemInstance.EquipSlot2 = reader.GetString("equip_slot"); //not sure what this is for

            itemInstance.IsUseableInTown = !reader.GetBoolean("no_use_in_town"); //not sure what this is for
            itemInstance.IsStorable = !reader.GetBoolean("no_storage");
            itemInstance.IsDiscardable = !reader.GetBoolean("no_discard");
            itemInstance.IsSellable = !reader.GetBoolean("no_sell");
            itemInstance.IsTradeable = !reader.GetBoolean("no_trade");
            itemInstance.IsTradableAfterUse = !reader.GetBoolean("no_trade_after_used");
            itemInstance.IsStealable = !reader.GetBoolean("no_stolen");

            itemInstance.IsGoldBorder = reader.GetBoolean("gold_border");

            //itemInstance.Lore = reader.GetString("lore");

            itemInstance.IconId = reader.GetInt32("icon");

            itemInstance.TalkRingName = "";
            //TODO fix all the data types once mysql is implemented
            itemInstance.BagSize = reader.GetByte("num_of_bag_slots");

            //grade,
            //weight
            itemInstance.Weight = (int)(reader.GetDouble("weight") * 10000); // TODO DOUBLE CHECK THIS IS CORRECT SCALE

            //auction
            itemInstance.ConsignerSoulName = reader.IsDBNull("consigner_soul_name") ? "" : reader.GetString("consigner_soul_name");
            itemInstance.SecondsUntilExpiryTime = reader.IsDBNull("expiry_datetime") ? 0 : CalcSecondsToExpiry(reader.GetInt64("expiry_datetime"));
            itemInstance.MinimumBid = reader.IsDBNull("min_bid") ? 0 : (ulong)reader.GetInt64("min_bid");
            itemInstance.BuyoutPrice = reader.IsDBNull("buyout_price") ? 0 : (ulong)reader.GetInt64("buyout_price");            
            itemInstance.Comment = reader.IsDBNull("comment") ? "" : reader.GetString("comment");

            return itemInstance;
        }

        public List<ItemInstance> SelectLootableInventoryItems(uint ownerId)
        {
            List<ItemInstance> lootableItemInstances = new List<ItemInstance>();
            ExecuteReader(SqlSelectOwnedInventoryItems,
                command =>
                {
                    AddParameter(command, "@owner_id", ownerId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        lootableItemInstances.Add(MakeItemInstance(reader));
                    }
                });
            return lootableItemInstances;
        }

        private int CalcSecondsToExpiry(long unixTimeSecondsExpiry)
        {
            DateTime dNow = DateTime.Now;
            DateTimeOffset dOffsetNow = new DateTimeOffset(dNow);
            return ((int)(unixTimeSecondsExpiry - dOffsetNow.ToUnixTimeSeconds()));
        }

        private long CalcExpiryTime(int secondsToExpiry)
        {
            DateTime dNow = DateTime.Now;
            DateTimeOffset dOffsetNow = new DateTimeOffset(dNow);
            return dOffsetNow.ToUnixTimeSeconds() + secondsToExpiry;
        }

        public List<ItemInstance> SelectAuctions(uint ownerSoulId)
        {
            List<ItemInstance> auctions = new List<ItemInstance>();
            int i = 0;
            ExecuteReader(SqlSelectAuctions,
                command => {
                    AddParameter(command, "@owner_soul_id", ownerSoulId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        ItemInstance itemInstance = MakeItemInstance(reader);
                        auctions.Add(itemInstance);
                    }
                });
            return auctions;
        }

        public void UpdateAuctionExhibit(ItemInstance itemInstance)
        {
            ExecuteNonQuery(SqlUpdateExhibit, command =>
            {
                AddParameter(command, "@id", itemInstance.InstanceID);
                AddParameter(command, "@consigner_soul_name", itemInstance.ConsignerSoulName);
                AddParameter(command, "@expiry_datetime", CalcExpiryTime(itemInstance.SecondsUntilExpiryTime));
                AddParameter(command, "@min_bid", itemInstance.MinimumBid);
                AddParameter(command, "@buyout_price", itemInstance.BuyoutPrice);
                AddParameter(command, "@comment", itemInstance.Comment);
            });
        }

        public void UpdateAuctionCancelExhibit(ulong instanceId)
        {
            ExecuteNonQuery(SqlUpdateCancelExhibit, command =>
            {
                AddParameter(command, "@id", instanceId);
                AddParameterNull(command, "@consigner_soul_name");
                AddParameterNull(command, "@expiry_datetime");
                AddParameterNull(command, "@min_bid");
                AddParameterNull(command, "@buyout_price");
                AddParameterNull(command, "@comment");
                AddParameterNull(command, "@bidder_soul_id");
            });
        }

        public List<ItemInstance> SelectBids(int bidderSoulId)
        {
            List<ItemInstance> bids = new List<ItemInstance>();
            int i = 0;
            ExecuteReader(SqlSelectBids,
                command =>
                {
                    AddParameter(command, "@bidder_soul_id", bidderSoulId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        ItemInstance itemInstance = MakeItemInstance(reader);                         
                        itemInstance.CurrentBid = reader.IsDBNull("current_bid") ? 0 : reader.GetInt32("current_bid");
                        itemInstance.BidderSoulId = reader.IsDBNull("bidder_soul_id") ? 0 : reader.GetInt32("bidder_soul_id");
                        itemInstance.MaxBid = reader.IsDBNull("max_bid") ? 0 : reader.GetInt32("max_bid");
                        bids.Add(itemInstance);
                    }
                });
            return bids;
        }

        public List<ItemInstance> SelectLots(int ownerSoulId)
        {
            List<ItemInstance> lots = new List<ItemInstance>();
            int i = 0;
            ExecuteReader(SqlSelectLots,
                command =>
                {
                    AddParameter(command, "@owner_soul_id", ownerSoulId);
                }, reader =>
                {
                    while (reader.Read())
                    {
                        ItemInstance itemInstance = MakeItemInstance(reader);
                        lots.Add(itemInstance);
                    }
                });
            return lots;
        }

        
    }
}
