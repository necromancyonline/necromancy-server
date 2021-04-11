using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodyAccessAbort : ClientHandler
    {
        public SendCharabodyAccessAbort(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_charabody_access_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            router.Send(client, (ushort) AreaPacketId.recv_charabody_access_end, res, ServerType.Area);
        }
    }
}
