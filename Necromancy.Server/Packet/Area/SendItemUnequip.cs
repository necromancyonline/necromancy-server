using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendItemUnequip : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendItemUnequip));

        public SendItemUnequip(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_item_unequip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemEquipSlots equipSlot = (ItemEquipSlots)(1 << packet.data.ReadInt32());

            ItemService itemService = new ItemService(client.character);
            int error = 0;
            _Logger.Debug(equipSlot.ToString());
            try
            {
                ItemInstance unequippedItem = itemService.CheckAlreadyEquipped(equipSlot);
                unequippedItem = itemService.Unequip(unequippedItem.currentEquipSlot);
                RecvItemUpdateEqMask recvItemUpdateEqMask = new RecvItemUpdateEqMask(client, unequippedItem);
                router.Send(recvItemUpdateEqMask);

                //notify other players of your new look
                RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.character, client.soul.name);
                router.Send(client.map, myCharacterData, client);
            }
            catch (ItemException e)
            {
                error = (int)e.type;
            }

            RecvItemUnequip recvItemUnequip = new RecvItemUnequip(client, error);
            router.Send(recvItemUnequip);

            //Re-do all your stats
            router.Send(client, itemService.CalculateBattleStats(client));
        }
    }
}
