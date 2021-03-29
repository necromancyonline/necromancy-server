using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_revert : ClientHandler
    {
        public send_trade_revert(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_revert;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check?
            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_revert_r, res, ServerType.Area);
            recvTradeNotifyRevert(client);
        }

        private void recvTradeNotifyRevert(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); //?

            Router.Send(client.Map, (ushort)AreaPacketId.recv_trade_notify_reverted, res, ServerType.Area, client);

        }
    }
}
