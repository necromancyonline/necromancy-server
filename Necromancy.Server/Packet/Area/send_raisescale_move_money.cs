using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_raisescale_move_money : ClientHandler
    {
        public send_raisescale_move_money(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_raisescale_move_money;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.Character.AdventureBagGold -= packet.Data.ReadUInt32();
            int errorCheck = packet.Data.ReadInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(errorCheck); //Error check
            Router.Send(client, (ushort) AreaPacketId.recv_raisescale_move_money_r, res, ServerType.Area);

        }
    }
}
