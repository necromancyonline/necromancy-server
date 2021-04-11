using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendUnionCloseWindow : ClientHandler
    {
        public SendUnionCloseWindow(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_union_close_window;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) AreaPacketId.recv_union_close_window_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteByte(0);
            router.Send(client, (ushort) AreaPacketId.recv_event_end, res2, ServerType.Area);
        }
    }
}
