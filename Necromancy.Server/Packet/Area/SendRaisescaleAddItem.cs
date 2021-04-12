using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendRaisescaleAddItem : ClientHandler
    {
        public SendRaisescaleAddItem(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_raisescale_add_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte bag = packet.data.ReadByte();
            byte unknown = packet.data.ReadByte(); //Type?
            int bagSlot = packet.data.ReadInt16();
            byte quantity = packet.data.ReadByte();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(bagSlot);
            router.Send(client, (ushort)AreaPacketId.recv_raisescale_add_item_r, res, ServerType.Area);
        }
    }
}
