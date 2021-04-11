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
    public class SendUnionStorageMoveItem : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendUnionStorageMoveItem));
        public SendUnionStorageMoveItem(NecServer server) : base(server) { }
        public override ushort id => (ushort) AreaPacketId.send_union_storage_move_item;
        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) AreaPacketId.recv_union_storage_move_item_r, res, ServerType.Area);

            byte fromStoreType = packet.data.ReadByte();
            byte fromBagId = packet.data.ReadByte();
            short fromSlot = packet.data.ReadInt16();
            ItemZoneType toZone = (ItemZoneType) packet.data.ReadByte();
            byte toBagId = packet.data.ReadByte();
            short toSlot = packet.data.ReadInt16();
            byte quantity = packet.data.ReadByte();

            _Logger.Debug($"fromStoreType byte [{fromStoreType}] toStoreType byte [{toZone}]");
            _Logger.Debug($"fromBagId byte [{fromBagId}] toBagIdId byte [{toBagId}]");
            _Logger.Debug($"fromSlot byte [{fromSlot}] toSlot [{toSlot}]");
            _Logger.Debug($"itemCount [{quantity}]");

            ItemLocation fromLoc = new ItemLocation((ItemZoneType)fromStoreType, fromBagId, fromSlot);
            ItemLocation toLoc = new ItemLocation(toZone, toBagId, toSlot);
            ItemService itemService = new ItemService(client.character);
            int error = 0;

            try
            {
                MoveResult moveResult = itemService.Move(fromLoc, toLoc, quantity);
                List<PacketResponse> responses = itemService.GetMoveResponses(client, moveResult);
                router.Send(client, responses);
            }
            catch (ItemException e) { error = (int)e.type; }

            RecvUnionStorageMoveItem recvUnionStorageMoveItem = new RecvUnionStorageMoveItem(client, error);
            router.Send(recvUnionStorageMoveItem);
        }
    }
}
