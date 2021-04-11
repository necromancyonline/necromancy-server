using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodySalvageRequestCancel : ClientHandler
    {
        public SendCharabodySalvageRequestCancel(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_charabody_salvage_request_cancel;

        public override void Handle(NecClient client, NecPacket packet)
        {

            RecvCharaBodySalvageRequestCancel recvCharaBodySalvageRequestCancel = new RecvCharaBodySalvageRequestCancel(0);
            router.Send(client, recvCharaBodySalvageRequestCancel.ToPacket());
        }
    }
}
