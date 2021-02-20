using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_raisescale_request_revive_event : ClientHandler
    {
        public send_raisescale_request_revive_event(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_raisescale_request_revive_event;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);  
            //Router.Send(client, (ushort) AreaPacketId.recv_raisescale_request_revive_r, res, ServerType.Area);

        }
    }
}
