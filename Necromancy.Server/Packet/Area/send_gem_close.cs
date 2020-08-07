using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_gem_close : ClientHandler
    {
        public send_gem_close(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_gem_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
	        res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_gem_close_r, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);

        }
    }
}