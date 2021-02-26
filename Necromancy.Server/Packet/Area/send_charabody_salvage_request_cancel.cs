using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_salvage_request_cancel : ClientHandler
    {
        public send_charabody_salvage_request_cancel(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_salvage_request_cancel;

        public override void Handle(NecClient client, NecPacket packet)
        {
         
            RecvCharaBodySalvageRequestCancel recvCharaBodySalvageRequestCancel = new RecvCharaBodySalvageRequestCancel(0);
            Router.Send(client, recvCharaBodySalvageRequestCancel.ToPacket());
        }
    }
}
