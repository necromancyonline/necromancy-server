using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendItemEquip : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendItemEquip));

        public SendItemEquip(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_item_equip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zone = (ItemZoneType)packet.data.ReadByte();
            byte bag = packet.data.ReadByte();
            short slot = packet.data.ReadInt16();
            ItemEquipSlots equipSlot = (ItemEquipSlots)packet.data.ReadInt32();

            _Logger.Debug($"storageType:{zone} bagId:{bag} bagSlotIndex:{slot} equipBit:{equipSlot}");

            ItemLocation location = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.character);
            int error = 0;

            try
            {
                if (equipSlot.HasFlag(ItemEquipSlots.LeftHand | ItemEquipSlots.RightHand)) //two handed weapon replaces 1h weapon and shield
                {
                    ItemInstance itemRight = itemService.CheckAlreadyEquipped(ItemEquipSlots.RightHand);
                    if (itemRight != null)
                    {
                        itemRight = itemService.Unequip(itemRight.currentEquipSlot);
                        itemRight.currentEquipSlot = ItemEquipSlots.None;
                        RecvItemUpdateEqMask recvItemUpdateEqMaskCurr = new RecvItemUpdateEqMask(client, itemRight);
                        router.Send(recvItemUpdateEqMaskCurr, client);
                    }

                    ItemInstance itemLeft = itemService.CheckAlreadyEquipped(ItemEquipSlots.LeftHand);
                    if (itemLeft != null)
                    {
                        itemLeft = itemService.Unequip(itemLeft.currentEquipSlot);
                        itemRight.currentEquipSlot = ItemEquipSlots.None;
                        RecvItemUpdateEqMask recvItemUpdateEqMaskCurr = new RecvItemUpdateEqMask(client, itemLeft);
                        router.Send(recvItemUpdateEqMaskCurr, client);
                    }
                }
                else //everything else besides 2h weapons.
                {
                    //update the equipment array
                    ItemInstance equippedItem = itemService.CheckAlreadyEquipped(equipSlot);
                    if (equippedItem != null)
                    {
                        equippedItem = itemService.Unequip(equippedItem.currentEquipSlot);
                        equippedItem.currentEquipSlot = ItemEquipSlots.None;
                        RecvItemUpdateEqMask recvItemUpdateEqMaskCurr = new RecvItemUpdateEqMask(client, equippedItem);
                        router.Send(recvItemUpdateEqMaskCurr, client);
                    }
                }

                //update the equipment array
                ItemInstance newEquippedItem = itemService.Equip(location, equipSlot);

                //Tell the client to move the icons to equipment slots
                RecvItemUpdateEqMask recvItemUpdateEqMask = new RecvItemUpdateEqMask(client, newEquippedItem);
                router.Send(recvItemUpdateEqMask);

                //notify other players of your new look
                RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.character, client.soul.name);
                router.Send(client.map, myCharacterData, client);
            }
            catch (ItemException e)
            {
                error = (int)e.type;
            }

            //tell the send if everything went well or not.  notify the client chat of any errors
            RecvItemEquip recvItemEquip = new RecvItemEquip(client, error);
            router.Send(recvItemEquip);

            //Re-do all your stats
            router.Send(client, itemService.CalculateBattleStats(client));
        }
    }
}
