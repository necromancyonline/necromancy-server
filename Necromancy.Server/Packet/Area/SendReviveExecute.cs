using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendReviveExecute : ClientHandler
    {
        public SendReviveExecute(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_revive_execute;

        public override void Handle(NecClient client, NecPacket packet)
        {
            RecvReviveExecute reviveExecute = new RecvReviveExecute();
            router.Send(reviveExecute, client);

            client.character.hp.SetCurrent(client.character.hp.max);
        }
    }
}
