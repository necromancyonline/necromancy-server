using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendReviveInit : ClientHandler
    {
        public SendReviveInit(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_revive_init;

        public override void Handle(NecClient client, NecPacket packet)
        {
            RecvReviveInit reviveInit = new RecvReviveInit();
            router.Send(reviveInit, client);
        }
    }
}
