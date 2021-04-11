using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_mentor_remove : ClientHandler
    {
        public send_party_mentor_remove(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_party_mentor_remove;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_party_mentor_remove_r, res, ServerType.Area);
        }
    }
}
