using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventUnionStorageClose : ClientHandler
    {
        public SendEventUnionStorageClose(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_event_union_storage_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) AreaPacketId.recv_event_union_storage_close_r, res, ServerType.Area);

            RecvEventEnd eventEnd = new RecvEventEnd(0);
            router.Send(eventEnd, client);
        }
    }
}
