using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendUnionRequestRename : ClientHandler
    {
        public SendUnionRequestRename(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_union_request_rename;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_union_request_rename_r, res, ServerType.Area);
        }
    }
}
