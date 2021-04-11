using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_raisescale_view_close_request : ClientHandler
    {
        public send_raisescale_view_close_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_raisescale_view_close_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            
            Router.Send(client, (ushort) AreaPacketId.recv_raisescale_view_close, res, ServerType.Area);

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res7, ServerType.Area);
        }
    }
}
