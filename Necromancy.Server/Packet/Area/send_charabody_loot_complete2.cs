using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_loot_complete2
 : ClientHandler
    {
        public send_charabody_loot_complete2(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_charabody_loot_complete2;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteFloat(0);

            Router.Send(client, (ushort)AreaPacketId.recv_charabody_loot_complete2_r, res, ServerType.Area);
        }
    }
}
