using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_loot_start2_cancel : ClientHandler
    {
        public send_charabody_loot_start2_cancel(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_loot_start2_cancel;

        public override void Handle(NecClient client, NecPacket packet)
        {


            //RecvCharaBodyNotifyLootStartCancel
        }
    }
}
