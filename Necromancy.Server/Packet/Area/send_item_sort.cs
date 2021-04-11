using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System;
using System.Collections.Generic;
using static Necromancy.Server.Systems.Item.ItemService;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_sort : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_item_sort));
        public send_item_sort(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_item_sort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zoneType = (ItemZoneType)packet.Data.ReadInt32();
            byte container = packet.Data.ReadByte();
            int itemsToSort = packet.Data.ReadInt32();

            Logger.Debug($"zoneType [{zoneType}] container [{container}] itemCount [{itemsToSort}]");

            short[] fromSlots = new short[itemsToSort];
            short[] toSlots = new short[itemsToSort];
            short[] quantities = new short[itemsToSort];
            for (int i = 0; i < itemsToSort; i++)
            {
                fromSlots[i] = packet.Data.ReadInt16();
                toSlots[i] = packet.Data.ReadInt16();
                quantities[i] = packet.Data.ReadInt16();

                Logger.Debug($"fromSlot short [{fromSlots[i]}] toSlot short [{toSlots[i]}] amount [{quantities[i]}]");
            }


            ItemService itemService = new ItemService(client.Character);
            int error = 0;

            try
            {
                for (int i = 0; i < itemsToSort; i++)
                {
                    ItemLocation fromLoc = new ItemLocation(zoneType, container, fromSlots[i]);
                    ItemLocation toLoc = new ItemLocation(zoneType, container, toSlots[i]);
                    byte quantity = (byte)quantities[i];

                    MoveResult moveResult = itemService.Move(fromLoc, toLoc, quantity);
                    List<PacketResponse> responses = itemService.GetMoveResponses(client, moveResult);
                    Router.Send(client, responses);
                }
            }
            catch (ItemException e) { error = (int)e.Type; }
            catch (Exception e1)
            {
                error = (int)ItemExceptionType.Generic;
                Logger.Exception(client, e1);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(error);
            Router.Send(client, (ushort)AreaPacketId.recv_item_sort_r, res, ServerType.Area);
        }
    }
}
