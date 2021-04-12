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
    public class send_item_unequip : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_item_unequip));
        public send_item_unequip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_unequip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemEquipSlots equipSlot = (ItemEquipSlots) (1 << packet.Data.ReadInt32()); 
            
            ItemService itemService = new ItemService(client.Character);
            int error = 0;
            Logger.Debug(equipSlot.ToString());
            try
            {
                ItemInstance unequippedItem = itemService.CheckAlreadyEquipped(equipSlot);
                unequippedItem = itemService.Unequip(unequippedItem.CurrentEquipSlot);
                RecvItemUpdateEqMask recvItemUpdateEqMask = new RecvItemUpdateEqMask(client, unequippedItem);
                Router.Send(recvItemUpdateEqMask);

                //notify other players of your new look
                RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
                Router.Send(client.Map, myCharacterData, client);
            }
            catch (ItemException e) { error = (int) e.Type; }

            RecvItemUnequip recvItemUnequip = new RecvItemUnequip(client, error);
            Router.Send(recvItemUnequip);

            //Re-do all your stats
            Router.Send(client, itemService.CalculateBattleStats(client));
        }
    }
}
