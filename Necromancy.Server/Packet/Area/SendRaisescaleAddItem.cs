using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_raisescale_add_item : ClientHandler
    {
        public send_raisescale_add_item(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_raisescale_add_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte bag = packet.Data.ReadByte();
            byte unknown = packet.Data.ReadByte(); //Type?
            int bagSlot = packet.Data.ReadInt16();
            byte quantity = packet.Data.ReadByte();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(bagSlot);
            Router.Send(client, (ushort) AreaPacketId.recv_raisescale_add_item_r, res, ServerType.Area);
        }
    }
}
