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
    public class send_union_storage_move_item : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_union_storage_move_item));
        public send_union_storage_move_item(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_union_storage_move_item;
        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_union_storage_move_item_r, res, ServerType.Area);

            byte fromStoreType = packet.Data.ReadByte();
            byte fromBagId = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            ItemZoneType toZone = (ItemZoneType) packet.Data.ReadByte();
            byte toBagId = packet.Data.ReadByte();
            short toSlot = packet.Data.ReadInt16();
            byte quantity = packet.Data.ReadByte();

            Logger.Debug($"fromStoreType byte [{fromStoreType}] toStoreType byte [{toZone}]");
            Logger.Debug($"fromBagId byte [{fromBagId}] toBagIdId byte [{toBagId}]");
            Logger.Debug($"fromSlot byte [{fromSlot}] toSlot [{toSlot}]");
            Logger.Debug($"itemCount [{quantity}]");

            ItemLocation fromLoc = new ItemLocation((ItemZoneType)fromStoreType, fromBagId, fromSlot);
            ItemLocation toLoc = new ItemLocation(toZone, toBagId, toSlot);
            ItemService itemService = new ItemService(client.Character);
            int error = 0;

            try
            {
                MoveResult moveResult = itemService.Move(fromLoc, toLoc, quantity);
                List<PacketResponse> responses = itemService.GetMoveResponses(client, moveResult);
                Router.Send(client, responses);
            }
            catch (ItemException e) { error = (int)e.ExceptionType; }

            RecvUnionStorageMoveItem recvUnionStorageMoveItem = new RecvUnionStorageMoveItem(client, error);
            Router.Send(recvUnionStorageMoveItem);
        }
    }
}
