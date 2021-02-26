using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_self_salvage_abort : ClientHandler
    {
        public send_charabody_self_salvage_abort(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_self_salvage_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient necClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            RecvCharaBodySelfSalvageEnd recvCharaBodySelfSalvageEnd = new RecvCharaBodySelfSalvageEnd(0);
            Router.Send(necClient, recvCharaBodySelfSalvageEnd.ToPacket());
        }
    }
}
