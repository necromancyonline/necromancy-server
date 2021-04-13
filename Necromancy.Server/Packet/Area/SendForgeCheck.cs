using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendForgeCheck : ClientHandler
    {
        public SendForgeCheck(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_forge_check;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte[] forgeItemStorageType = new byte[3];
            byte[] forgeItemBag = new byte[3];
            short[] forgeItemSlot = new short[3];

            byte storageType = packet.data.ReadByte();
            byte bag = packet.data.ReadByte();
            short slot = packet.data.ReadInt16();
            int forgeItemCount = packet.data.ReadInt32();
            for (int i = 0; i < forgeItemCount; i++)
            {
                forgeItemStorageType[i] = packet.data.ReadByte();
                forgeItemBag[i] = packet.data.ReadByte();
                forgeItemSlot[i] = packet.data.ReadInt16();
            }

            byte supportItemCount = packet.data.ReadByte();
            byte supportItemStorageType = packet.data.ReadByte();
            byte supportItemBag = packet.data.ReadByte();
            short supportItemSlot = packet.data.ReadInt16();

            //5 bytes left
            //TODO

            ItemService itemService = new ItemService(client.character);
            ItemInstance itemInstance = client.character.itemLocationVerifier.GetItem(new ItemLocation((ItemZoneType)storageType, bag, slot));
            ForgeMultiplier forgeMultiplier = itemService.ForgeMultiplier(itemInstance.enhancementLevel + 1);

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); //err check
            res.WriteUInt64((ulong)Util.GetRandomNumber(500, 600)); //Forge Cost.   Not itemInstance.InstanceID
            res.WriteByte((byte)Util.GetRandomNumber(0, 11)); //'Masters Response' Probability of success cast as a sarcastic message.
            res.WriteInt32(itemInstance.physical * 100);
            res.WriteInt32(itemInstance.magical * 100);
            res.WriteInt32(itemInstance.maximumDurability);
            res.WriteByte(itemInstance.hardness);
            res.WriteInt32((int)(itemInstance.physical * forgeMultiplier.factor) * 100);
            res.WriteInt32((int)(itemInstance.magical * forgeMultiplier.factor) * 100);
            res.WriteInt32((int)(itemInstance.maximumDurability * forgeMultiplier.durability));
            res.WriteByte((byte)(itemInstance.hardness + forgeMultiplier.hardness));
            res.WriteInt32(itemInstance.weight - forgeMultiplier.weight);
            res.WriteInt16((short)Util.GetRandomNumber(0, 5)); //???
            res.WriteInt16((short)Util.GetRandomNumber(0, 5)); //???
            res.WriteInt16((short)Util.GetRandomNumber(0, 5)); //???

            router.Send(client, (ushort)AreaPacketId.recv_forge_check_r, res, ServerType.Area);
        }
    }
}
