using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_random_box_close : ClientHandler
    {
        public send_random_box_close(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_random_box_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_random_box_close_r, res, ServerType.Area);
        }
    }
}
