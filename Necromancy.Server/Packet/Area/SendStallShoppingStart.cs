using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_stall_shopping_start : ClientHandler
    {
        public send_stall_shopping_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_stall_shopping_start;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int stallOwnerCharacterID = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_stall_shopping_start_r, res, ServerType.Area);
        }
    }
}
