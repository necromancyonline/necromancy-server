using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_equip : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_item_equip));
        public send_item_equip(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_item_equip;
        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zone = (ItemZoneType) packet.Data.ReadByte();
            byte bag = packet.Data.ReadByte();
            short slot = packet.Data.ReadInt16();
            ItemEquipSlots equipSlot = (ItemEquipSlots) packet.Data.ReadInt32();
            
            Logger.Debug($"storageType:{zone} bagId:{bag} bagSlotIndex:{slot} equipBit:{equipSlot}");

            ItemLocation location = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.Character);            
            int error = 0;

            try
            {
                if (equipSlot.HasFlag(ItemEquipSlots.LeftHand | ItemEquipSlots.RightHand)) //two handed weapon replaces 1h weapon and shield
                {
                    ItemInstance itemRight = itemService.CheckAlreadyEquipped(ItemEquipSlots.RightHand);
                    if (itemRight != null)
                    { 
                        itemRight = itemService.Unequip(itemRight.CurrentEquipSlot);
                        itemRight.CurrentEquipSlot = ItemEquipSlots.None;
                        RecvItemUpdateEqMask recvItemUpdateEqMaskCurr = new RecvItemUpdateEqMask(client, itemRight);
                        Router.Send(recvItemUpdateEqMaskCurr, client);
                    }
                    ItemInstance itemLeft = itemService.CheckAlreadyEquipped(ItemEquipSlots.LeftHand);
                    if (itemLeft != null)
                    { 
                        itemLeft = itemService.Unequip(itemLeft.CurrentEquipSlot);
                        itemRight.CurrentEquipSlot = ItemEquipSlots.None;
                        RecvItemUpdateEqMask recvItemUpdateEqMaskCurr = new RecvItemUpdateEqMask(client, itemLeft);
                        Router.Send(recvItemUpdateEqMaskCurr, client);
                    }
                }
                else //everything else besides 2h weapons. 
                {
                    //update the equipment array
                    ItemInstance equippedItem = itemService.CheckAlreadyEquipped(equipSlot);
                    if (equippedItem != null)
                    {
                        equippedItem = itemService.Unequip(equippedItem.CurrentEquipSlot);
                        equippedItem.CurrentEquipSlot = ItemEquipSlots.None;
                        RecvItemUpdateEqMask recvItemUpdateEqMaskCurr = new RecvItemUpdateEqMask(client, equippedItem);
                        Router.Send(recvItemUpdateEqMaskCurr, client);
                    }
                }

                //update the equipment array
                ItemInstance newEquippedItem = itemService.Equip(location, equipSlot);

                //Tell the client to move the icons to equipment slots
                RecvItemUpdateEqMask recvItemUpdateEqMask = new RecvItemUpdateEqMask(client, newEquippedItem);
                Router.Send(recvItemUpdateEqMask);

                //notify other players of your new look
                RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
                Router.Send(client.Map, myCharacterData, client);
            }
            catch (ItemException e) { error = (int) e.ExceptionType; }

            //tell the send if everything went well or not.  notify the client chat of any errors
            RecvItemEquip recvItemEquip = new RecvItemEquip(client, error);
            Router.Send(recvItemEquip);

            short PhysAttack = 0;
            short MagAttack = 0;
            short PhysDef = 0;
            short MagDef = 0;
            client.Character.Weight.setCurrent(0);

            foreach (ItemInstance itemInstance2 in client.Character.EquippedItems.Values)
            {
                if (itemInstance2.CurrentEquipSlot.HasFlag(ItemEquipSlots.RightHand) | itemInstance2.CurrentEquipSlot == ItemEquipSlots.Quiver)
                {
                    PhysAttack += (short)itemInstance2.Physical;
                    MagAttack += (short)itemInstance2.Magical;
                }
                else
                {
                    PhysDef += (short)itemInstance2.Physical;
                    MagDef += (short)itemInstance2.Magical;
                }
                client.Character.Weight.Modify(itemInstance2.Weight);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.Weight.max); //max weight
            res.WriteInt32(client.Character.Weight.current); //diff weight
            Router.Send(client, (ushort)AreaPacketId.recv_chara_update_maxweight, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(client.Character.Weight.current); //diff weight
            Router.Send(client, (ushort)AreaPacketId.recv_chara_update_weight, res, ServerType.Area);




            res = BufferProvider.Provide();
            res.WriteInt16((short)client.Character.Strength); //base Phys Attack
            res.WriteInt16((short)client.Character.Dexterity); //base Phys Def
            res.WriteInt16((short)client.Character.Intelligence); //base Mag attack
            res.WriteInt16((short)client.Character.Piety); //base Mag Def

            res.WriteInt16((short)client.Character.Luck); //Ranged physical attack

            res.WriteInt16(PhysAttack); //Equip Bonus Phys attack
            res.WriteInt16(PhysDef); //Equip bonus Phys Def
            res.WriteInt16(MagAttack); //Equip Bonus Mag Attack
            res.WriteInt16(MagDef); //Equip bonus Mag Def

            res.WriteInt16(777); //Ranged physical attack

            Router.Send(client, (ushort)AreaPacketId.recv_chara_update_battle_base_param, res, ServerType.Area);
        }
    }
}
