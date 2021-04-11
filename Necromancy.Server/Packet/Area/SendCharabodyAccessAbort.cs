using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_access_abort : ClientHandler
    {
        public send_charabody_access_abort(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_access_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_charabody_access_end, res, ServerType.Area);            
        }
    }
}
