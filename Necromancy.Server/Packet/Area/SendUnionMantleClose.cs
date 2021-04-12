using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendUnionMantleClose : ClientHandler
    {
        public SendUnionMantleClose(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_union_mantle_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client.map, (ushort)AreaPacketId.recv_union_mantle_close_r, res, ServerType.Area);
        }
    }
}
