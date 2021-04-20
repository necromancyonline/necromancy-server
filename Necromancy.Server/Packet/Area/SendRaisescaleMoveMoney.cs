using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendRaisescaleMoveMoney : ClientHandler
    {
        public SendRaisescaleMoveMoney(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_raisescale_move_money;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.character.adventureBagGold -= packet.data.ReadUInt32();
            int errorCheck = packet.data.ReadInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(errorCheck); //Error check
            router.Send(client, (ushort)AreaPacketId.recv_raisescale_move_money_r, res, ServerType.Area);
        }
    }
}
