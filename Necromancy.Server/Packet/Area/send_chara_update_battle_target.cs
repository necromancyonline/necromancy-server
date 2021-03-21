using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_chara_update_battle_target : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_chara_update_battle_target));

        public send_chara_update_battle_target(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_chara_update_battle_target;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint objectId = packet.Data.ReadUInt32();
            Logger.Debug($"Targeting : {objectId}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            //Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_bid_r, res, ServerType.Area);
        }
    }
}
