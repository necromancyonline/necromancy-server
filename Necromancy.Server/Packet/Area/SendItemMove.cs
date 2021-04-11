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
    public class SendItemMove : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendItemMove));
        public SendItemMove(NecServer server) : base(server) { }
        public override ushort id => (ushort) AreaPacketId.send_item_move;
        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType fromZone = (ItemZoneType) packet.data.ReadByte();
            byte fromContainer = packet.data.ReadByte();
            short fromSlot = packet.data.ReadInt16();
            ItemZoneType toZone = (ItemZoneType) packet.data.ReadByte();
            byte toContainer = packet.data.ReadByte();
            short toSlot = packet.data.ReadInt16();
            byte quantity = packet.data.ReadByte();

            _Logger.Debug($"fromZoneType byte [{fromZone}] toZoneType byte [{toZone}]");
            _Logger.Debug($"fromContainerId byte [{fromContainer}] toContainerId byte [{toContainer}]");
            _Logger.Debug($"fromSlot byte [{fromSlot}] toSlot[{ toSlot}]");
            _Logger.Debug($"itemCount [{quantity}]");

            ItemLocation fromLoc = new ItemLocation(fromZone, fromContainer, fromSlot);
            ItemLocation toLoc = new ItemLocation(toZone, toContainer, toSlot);
            ItemService itemService = new ItemService(client.character);
            int error = 0;

            try
            {
                MoveResult moveResult = itemService.Move(fromLoc, toLoc, quantity);
                List<PacketResponse> responses = itemService.GetMoveResponses(client, moveResult);
                router.Send(client,responses);
            }
            catch (ItemException e) { error = (int) e.type; }

            RecvItemMove recvItemMove = new RecvItemMove(client, error);
            router.Send(recvItemMove);
        }
    }
}
