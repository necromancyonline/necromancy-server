using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyPassDraw : ClientHandler
    {
        public SendPartyPassDraw(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_party_pass_draw;

        public override void Handle(NecClient client, NecPacket packet)
        {
            long targetItemId = packet.data.ReadInt64();

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.character.instanceId);

            //Router.Send(client, (ushort) AreaPacketId.recv_party_pass_draw_r, res, ServerType.Area);  //need to find pass draw receive and opcode
        }
    }
}
