using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_wanted_list_close : ClientHandler
    {
        public send_wanted_list_close(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_wanted_list_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_wanted_list_close_r, res, ServerType.Area);
        }
    }
}
