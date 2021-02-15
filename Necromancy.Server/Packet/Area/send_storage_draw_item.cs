using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;
using static Necromancy.Server.Systems.Item.ItemService;

namespace Necromancy.Server.Packet.Area
{
    public class send_storage_draw_item : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_storage_draw_item));
        public send_storage_draw_item(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_storage_draw_item;
        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType fromZone = (ItemZoneType) packet.Data.ReadByte();
            byte fromBag = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            ItemZoneType toZone = (ItemZoneType) packet.Data.ReadByte();
            byte toBag = packet.Data.ReadByte();
            short toSlot = packet.Data.ReadInt16();
            byte quantity = packet.Data.ReadByte();

            Logger.Debug($"fromStoreType byte [{fromZone}] toStoreType byte [{toZone}]");
            Logger.Debug($"fromBagId byte [{fromBag}] toBagId byte [{toBag}]");
            Logger.Debug($"fromSlot byte [{fromSlot}] toSlot[{ toSlot}]");
            Logger.Debug($"itemCount [{quantity}]");

            ItemLocation fromLoc = new ItemLocation(fromZone, fromBag, fromSlot);
            ItemLocation toLoc = new ItemLocation(toZone, toBag, toSlot);
            ItemService itemService = new ItemService(client.Character);
            int error = 0;

            try
            {
                MoveResult moveResult = itemService.Move(fromLoc, toLoc, quantity);
                List<PacketResponse> responses = itemService.GetMoveResponses(client, moveResult);
                Router.Send(client, responses);
            }
            catch (ItemException e) { error = (int)e.ExceptionType; }

            RecvStorageDrawItem2 recvStorageDrawItem2 = new RecvStorageDrawItem2(client, error);
            Router.Send(recvStorageDrawItem2);
        }        
    }
}
