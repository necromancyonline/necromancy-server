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
                ItemInstance equippedItem = itemService.Equip(location, equipSlot);                
                RecvItemUpdateEqMask recvItemUpdateEqMask = new RecvItemUpdateEqMask(client, equippedItem);
                Router.Send(recvItemUpdateEqMask);
            }
            catch (ItemException e) { error = (int) e.ExceptionType; }

            RecvItemEquip recvItemEquip = new RecvItemEquip(client, error);
            Router.Send(recvItemEquip);
        }
    }
}
