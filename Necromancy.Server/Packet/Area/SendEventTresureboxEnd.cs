using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventTresureboxEnd : ClientHandler
    {
        public SendEventTresureboxEnd(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_event_treasurebox_end;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
        }
    }
}
