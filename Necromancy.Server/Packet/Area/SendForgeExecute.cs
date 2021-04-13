using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendForgeExecute : ClientHandler
    {
        public SendForgeExecute(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_forge_execute;

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

            int result = 1;

            int luckyChance = Util.GetRandomNumber(0, client.character.luck); //the more luck you have, the better your chances
            if (luckyChance < 5) result = 2;
            if ((result == 2) & (forgeItemCount > 1) && Util.GetRandomNumber(0, client.character.luck) < 4) result = 1; // use 2 forge stone, get a 2nd chance
            if ((result == 2) & (forgeItemCount > 2) && Util.GetRandomNumber(0, client.character.luck) < 3) result = 1; // use 3 forge stone, get a 3rd chance
            //if ((result == 2) & (forgeItemCount > 2) && Util.GetRandomNumber(0, client.Character.Luck) < 5) result = 1; // use 3 forge stone, get a 4th chance

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // 0 is a pass, anything but 0 is a fail; seems like a check for if you can still upgrade the weapon
            res.WriteInt32(result); // anything but a 1 here is a fail condition, 1 here is a pass condition.
            router.Send(client, (ushort)AreaPacketId.recv_forge_execute_r, res, ServerType.Area);


            RecvForgeNotifyExecuteResult recvForgeNotifyExecuteResult = new RecvForgeNotifyExecuteResult(client.character.instanceId, result);
            router.Send(client, recvForgeNotifyExecuteResult.ToPacket());

            ItemService itemService = new ItemService(client.character);
            ItemInstance itemInstance = client.character.itemLocationVerifier.GetItem(new ItemLocation((ItemZoneType)storageType, bag, slot));
            ForgeMultiplier forgeMultiplier = itemService.ForgeMultiplier(itemInstance.enhancementLevel + 1);


            itemInstance.enhancementLevel += 1;
            itemInstance.physical = (short)(itemInstance.physical * forgeMultiplier.factor);
            itemInstance.magical = (short)(itemInstance.magical * forgeMultiplier.factor);
            itemInstance.maximumDurability = (short)(itemInstance.maximumDurability * forgeMultiplier.durability);
            itemInstance.hardness = (byte)(itemInstance.hardness + forgeMultiplier.hardness);
            itemInstance.weight = (short)(itemInstance.weight - forgeMultiplier.weight);

            if (result == 1)
            {
                RecvItemUpdateLevel recvItemUpdateLevel = new RecvItemUpdateLevel(itemInstance.instanceId, itemInstance.enhancementLevel);
                router.Send(client, recvItemUpdateLevel.ToPacket());
                RecvItemUpdatePhysics recvItemUpdatePhysics = new RecvItemUpdatePhysics(itemInstance.instanceId, itemInstance.physical);
                router.Send(client, recvItemUpdatePhysics.ToPacket());
                RecvItemUpdateMagic recvItemUpdateMagic = new RecvItemUpdateMagic(itemInstance.instanceId, itemInstance.magical);
                router.Send(client, recvItemUpdateMagic.ToPacket());
                RecvItemUpdateMaxDur recvItemUpdateMaxDur = new RecvItemUpdateMaxDur(itemInstance.instanceId, itemInstance.maximumDurability);
                router.Send(client, recvItemUpdateMaxDur.ToPacket());
                RecvItemUpdateHardness recvItemUpdateHardness = new RecvItemUpdateHardness(itemInstance.instanceId, itemInstance.hardness);
                router.Send(client, recvItemUpdateHardness.ToPacket());
                RecvItemUpdateWeight recvItemUpdateWeight = new RecvItemUpdateWeight(itemInstance.instanceId, itemInstance.weight);
                router.Send(client, recvItemUpdateWeight.ToPacket());

                itemService.UpdateEnhancementLevel(itemInstance);
            }
            else if (supportItemCount == 0)
            {
                itemService.Remove(itemInstance.location, itemInstance.quantity);
                RecvItemRemove recvItemRemove = new RecvItemRemove(client, itemInstance);
                router.Send(recvItemRemove);
            }
            else if (itemInstance.enhancementLevel > 4) //if forge fails but gaurd exists, do not lose item.
            {
                itemInstance.enhancementLevel = 4;

                ForgeMultiplier forgeDehancementMultiplier = itemService.LoginLoadMultiplier(itemInstance.enhancementLevel);
                server.settingRepository.itemLibrary.TryGetValue(itemInstance.baseId, out ItemLibrarySetting itemLibrarySetting); //ToDo,  load Nec_Item_Library into memory for queries like this.
                if (itemLibrarySetting != null)
                {
                    itemInstance.physical = (short)itemLibrarySetting.physicalAttack;
                    itemInstance.magical = (short)itemLibrarySetting.magicalAttack;
                    itemInstance.maximumDurability = itemLibrarySetting.durability;
                    itemInstance.hardness = (byte)itemLibrarySetting.hardness;
                    itemInstance.weight = (int)itemLibrarySetting.weight;
                }

                itemInstance.physical = (short)(itemInstance.physical * forgeDehancementMultiplier.factor);
                itemInstance.magical = (short)(itemInstance.magical * forgeDehancementMultiplier.factor);
                itemInstance.maximumDurability = (short)(itemInstance.maximumDurability * forgeDehancementMultiplier.durability);
                itemInstance.hardness = (byte)(itemInstance.hardness + forgeDehancementMultiplier.hardness);
                itemInstance.weight = (short)(itemInstance.weight - forgeDehancementMultiplier.weight);

                RecvItemUpdateLevel recvItemUpdateLevel = new RecvItemUpdateLevel(itemInstance.instanceId, itemInstance.enhancementLevel);
                router.Send(client, recvItemUpdateLevel.ToPacket());
                RecvItemUpdatePhysics recvItemUpdatePhysics = new RecvItemUpdatePhysics(itemInstance.instanceId, itemInstance.physical);
                router.Send(client, recvItemUpdatePhysics.ToPacket());
                RecvItemUpdateMagic recvItemUpdateMagic = new RecvItemUpdateMagic(itemInstance.instanceId, itemInstance.magical);
                router.Send(client, recvItemUpdateMagic.ToPacket());
                RecvItemUpdateMaxDur recvItemUpdateMaxDur = new RecvItemUpdateMaxDur(itemInstance.instanceId, itemInstance.maximumDurability);
                router.Send(client, recvItemUpdateMaxDur.ToPacket());
                RecvItemUpdateHardness recvItemUpdateHardness = new RecvItemUpdateHardness(itemInstance.instanceId, itemInstance.hardness);
                router.Send(client, recvItemUpdateHardness.ToPacket());
                RecvItemUpdateWeight recvItemUpdateWeight = new RecvItemUpdateWeight(itemInstance.instanceId, itemInstance.weight);
                router.Send(client, recvItemUpdateWeight.ToPacket());

                itemService.UpdateEnhancementLevel(itemInstance);
            }

            for (int i = 0; i < forgeItemCount; i++)
            {
                ItemInstance forgeItemInstance = client.character.itemLocationVerifier.GetItem(new ItemLocation((ItemZoneType)forgeItemStorageType[i], forgeItemBag[i], forgeItemSlot[i]));
                if (forgeItemInstance != null)
                {
                    RecvItemRemove recvItemRemove = new RecvItemRemove(client, forgeItemInstance);
                    router.Send(recvItemRemove);
                    itemService.Remove(forgeItemInstance.location, forgeItemInstance.quantity);
                }
            }

            for (int i = 0; i < supportItemCount; i++)
            {
                ItemInstance supportItemInstance = client.character.itemLocationVerifier.GetItem(new ItemLocation((ItemZoneType)supportItemStorageType, supportItemBag, supportItemSlot));
                if (supportItemInstance != null)
                {
                    RecvItemRemove recvItemRemove = new RecvItemRemove(client, supportItemInstance);
                    router.Send(recvItemRemove);
                    itemService.Remove(supportItemInstance.location, supportItemInstance.quantity);
                }
            }
        }
    }
}
