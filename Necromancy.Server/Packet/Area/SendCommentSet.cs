using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_comment_set : ClientHandler
    {
        public send_comment_set(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_comment_set;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

          
            res.WriteInt32(0); 

            Router.Send(client, (ushort) AreaPacketId.recv_comment_set_r, res, ServerType.Area);
        }
    }
}
