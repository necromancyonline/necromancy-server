using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_pass_draw : ClientHandler
    {
        public send_party_pass_draw(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_pass_draw;

        public override void Handle(NecClient client, NecPacket packet)
        {
            long targetItemId = packet.Data.ReadInt64();

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.Character.InstanceId);

            //Router.Send(client, (ushort) AreaPacketId.recv_party_pass_draw_r, res, ServerType.Area);  //need to find pass draw receive and opcode
        }
    }
}
