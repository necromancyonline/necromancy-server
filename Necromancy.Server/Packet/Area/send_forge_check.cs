using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_forge_check : ClientHandler
    {
        public send_forge_check(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_forge_check;
        public override void Handle(NecClient client, NecPacket packet)
        {
            byte[] forgeItemStorageType = new byte[3];
            byte[] forgeItemBag = new byte[3];
            short[] forgeItemSlot = new short[3];

            byte storageType = packet.Data.ReadByte();
            byte Bag = packet.Data.ReadByte();
            short Slot = packet.Data.ReadInt16();
            int forgeItemCount = packet.Data.ReadInt32();
            for (int i = 0; i<forgeItemCount; i++)
            {
                forgeItemStorageType[i] = packet.Data.ReadByte();
                forgeItemBag[i] = packet.Data.ReadByte();
                forgeItemSlot[i] = packet.Data.ReadInt16();
            }
            byte supportItemCount = packet.Data.ReadByte();
            byte supportItemStorageType = packet.Data.ReadByte();
            byte supportItemBag = packet.Data.ReadByte();
            short supportItemSlot = packet.Data.ReadInt16();

            //5 bytes left
            //TODO

            ItemService itemService = new ItemService(client.Character);
            ItemInstance inventoryItem = client.Character.ItemManager.GetItem(new ItemLocation((ItemZoneType)storageType, Bag, Slot));
            Server.SettingRepository.ItemLibrary.TryGetValue(inventoryItem.BaseID, out ItemLibrarySetting itemLibrarySetting);

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); //err check
            res.WriteUInt64(inventoryItem.InstanceID);
            res.WriteByte(inventoryItem.Quantity);
            res.WriteInt32(inventoryItem.Physical*100);
            res.WriteInt32(inventoryItem.Magical*100);
            res.WriteInt32(itemLibrarySetting.Durability/*Max Durability*/);
            res.WriteByte((byte)itemLibrarySetting.Hardness);
            res.WriteInt32(inventoryItem.Physical*200);
            res.WriteInt32(inventoryItem.Magical*200);
            res.WriteInt32(inventoryItem.MaximumDurability + 10);
            res.WriteByte((byte)(inventoryItem.Hardness + 1));
            res.WriteInt32(inventoryItem.Weight - 100);
            res.WriteInt16((short)(inventoryItem.GP + 5)); //???
            res.WriteInt16((short)(inventoryItem.GP + 10));//??
            res.WriteInt16((short)(inventoryItem.GP + 15));//??

            Router.Send(client, (ushort)AreaPacketId.recv_forge_check_r, res, ServerType.Area);
        }
    }
}
