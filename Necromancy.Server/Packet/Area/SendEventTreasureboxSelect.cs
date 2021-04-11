using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventTreasureboxSelect : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendItemMove));
        public SendEventTreasureboxSelect(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_event_treasurebox_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte fromStoreType = packet.data.ReadByte();
            byte fromBagId = packet.data.ReadByte();
            short fromSlot = packet.data.ReadInt16();
            _Logger.Debug($"fromStoreType byte [{fromStoreType}]");
            _Logger.Debug($"fromBagId byte [{fromBagId}]");
            _Logger.Debug($"fromSlot byte [{fromSlot}]");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client.map, (ushort) AreaPacketId.recv_event_treasurebox_select_r, res, ServerType.Area);

            //insert logic to grab Item from Inventory based on above data.Read  here
            long itemId = 0;
            byte toStoreType = 0;
            byte toBagId = 0;
            short toSlot = 0;


            res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt64(itemId); // item id
            res.WriteByte(toStoreType); // 0 = adventure bag. 1 = character equipment, 2 = royal bag
            res.WriteByte(toBagId); // position 2	cause crash if you change the 0	]	} im assumming these are x/y row, and page
            res.WriteInt16(toSlot); // bag index 0 to 24
            router.Send(client, (ushort)AreaPacketId.recv_item_update_place, res, ServerType.Area);

            res = BufferProvider.Provide();
            router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);

        }
    }
}
