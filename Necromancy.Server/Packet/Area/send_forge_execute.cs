using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_forge_execute : ClientHandler
    {
        public send_forge_execute(NecServer server) : base(server)
        {
        }
        public override ushort Id => (ushort) AreaPacketId.send_forge_execute;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte[] forgeItemStorageType = new byte[3];
            byte[] forgeItemBag = new byte[3];
            short[] forgeItemSlot = new short[3];

            byte storageType = packet.Data.ReadByte();
            byte Bag = packet.Data.ReadByte();
            short Slot = packet.Data.ReadInt16();
            int forgeItemCount = packet.Data.ReadInt32();
            for (int i = 0; i < forgeItemCount; i++)
            {
                forgeItemStorageType[i] = packet.Data.ReadByte();
                forgeItemBag[i] = packet.Data.ReadByte();
                forgeItemSlot[i] = packet.Data.ReadInt16();
            }
            byte supportItemCount = packet.Data.ReadByte();
            byte supportItemStorageType = packet.Data.ReadByte();
            byte supportItemBag = packet.Data.ReadByte();
            short supportItemSlot = packet.Data.ReadInt16();

            int result = 1;

            int luckyChance = Util.GetRandomNumber(0, client.Character.Luck); //the more luck you have, the better your chances
            if (luckyChance < 5) result = 2;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);// 0 is a pass, anything but 0 is a fail; seems like a check for if you can still upgrade the weapon
            res.WriteInt32(result);// anything but a 1 here is a fail condition, 1 here is a pass condition.
            Router.Send(client, (ushort)AreaPacketId.recv_forge_execute_r, res, ServerType.Area);



            RecvForgeNotifyExecuteResult recvForgeNotifyExecuteResult = new RecvForgeNotifyExecuteResult(client.Character.InstanceId, result);
            Router.Send(client, recvForgeNotifyExecuteResult.ToPacket());

            ItemService itemService = new ItemService(client.Character);
            ItemInstance itemInstance = client.Character.ItemManager.GetItem(new ItemLocation((ItemZoneType)storageType, Bag, Slot));
            ForgeMultiplier forgeMultiplier = itemService.ForgeMultiplier(itemInstance.EnhancementLevel + 1);


            itemInstance.EnhancementLevel += 1;
            itemInstance.Physical = (short)(itemInstance.Physical * forgeMultiplier.Factor);
            itemInstance.Magical = (short)(itemInstance.Magical * forgeMultiplier.Factor);
            itemInstance.MaximumDurability = (short)(itemInstance.MaximumDurability * forgeMultiplier.Durability);
            itemInstance.Hardness = (byte)(itemInstance.Hardness + forgeMultiplier.Hardness);
            itemInstance.Weight = (short)(itemInstance.Weight - forgeMultiplier.Weight);

            if (result == 1)
            { 
            RecvItemUpdateLevel recvItemUpdateLevel = new RecvItemUpdateLevel(itemInstance.InstanceID, itemInstance.EnhancementLevel);
            Router.Send(client, recvItemUpdateLevel.ToPacket());
            RecvItemUpdatePhysics recvItemUpdatePhysics = new RecvItemUpdatePhysics(itemInstance.InstanceID, itemInstance.Physical);
            Router.Send(client, recvItemUpdatePhysics.ToPacket());
            RecvItemUpdateMagic recvItemUpdateMagic = new RecvItemUpdateMagic(itemInstance.InstanceID, itemInstance.Magical);
            Router.Send(client, recvItemUpdateMagic.ToPacket());
            RecvItemUpdateMaxDur recvItemUpdateMaxDur = new RecvItemUpdateMaxDur(itemInstance.InstanceID, itemInstance.MaximumDurability);
            Router.Send(client, recvItemUpdateMaxDur.ToPacket());
            RecvItemUpdateHardness recvItemUpdateHardness = new RecvItemUpdateHardness(itemInstance.InstanceID, itemInstance.Hardness);
            Router.Send(client, recvItemUpdateHardness.ToPacket());
            RecvItemUpdateWeight recvItemUpdateWeight = new RecvItemUpdateWeight(itemInstance.InstanceID, itemInstance.Weight);
            Router.Send(client, recvItemUpdateWeight.ToPacket());

            itemService.UpdateEnhancementLevel(itemInstance);

            }
            else
            {
                RecvItemRemove recvItemRemove = new RecvItemRemove(client, itemInstance);
                Router.Send(recvItemRemove);
            }
            for (int i = 0; i < forgeItemCount; i++)
            {
                ItemInstance forgeItemInstance = client.Character.ItemManager.GetItem(new ItemLocation((ItemZoneType)forgeItemStorageType[i], forgeItemBag[i], forgeItemSlot[i]));
                RecvItemRemove recvItemRemove = new RecvItemRemove(client, forgeItemInstance);
                Router.Send(recvItemRemove);

            }
            for (int i = 0; i < supportItemCount; i++)
            {
                ItemInstance supportItemInstance = client.Character.ItemManager.GetItem(new ItemLocation((ItemZoneType)supportItemStorageType, supportItemBag, supportItemSlot));
                RecvItemRemove recvItemRemove = new RecvItemRemove(client, supportItemInstance);
                Router.Send(recvItemRemove);
            }



        }
    }
}
