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
    public class SendItemSort : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendItemSort));
        public SendItemSort(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_item_sort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zoneType = (ItemZoneType)packet.data.ReadInt32();
            byte container = packet.data.ReadByte();
            int itemsToSort = packet.data.ReadInt32();

            _Logger.Debug($"zoneType [{zoneType}] container [{container}] itemCount [{itemsToSort}]");

            short[] fromSlots = new short[itemsToSort];
            short[] toSlots = new short[itemsToSort];
            short[] quantities = new short[itemsToSort];
            for (int i = 0; i < itemsToSort; i++)
            {
                fromSlots[i] = packet.data.ReadInt16();
                toSlots[i] = packet.data.ReadInt16();
                quantities[i] = packet.data.ReadInt16();

                _Logger.Debug($"fromSlot short [{fromSlots[i]}] toSlot short [{toSlots[i]}] amount [{quantities[i]}]");
            }


            ItemService itemService = new ItemService(client.character);
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
                    router.Send(client, responses);
                }
            }
            catch (ItemException e) { error = (int)e.type; }
            catch (Exception e1)
            {
                error = (int)ItemExceptionType.Generic;
                _Logger.Exception(client, e1);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(error);
            router.Send(client, (ushort)AreaPacketId.recv_item_sort_r, res, ServerType.Area);
        }
    }
}
