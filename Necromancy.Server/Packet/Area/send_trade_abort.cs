using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_abort : ClientHandler
    {
        public send_trade_abort(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_trade_abort_r, res, ServerType.Area);

            RecvEventEnd eventEnd = new RecvEventEnd(0);
            if (targetClient != null)
            {
                RecvTradeNotifyAborted notifyAborted = new RecvTradeNotifyAborted();
                Router.Send(notifyAborted, targetClient);
                Router.Send(eventEnd, targetClient);
                targetClient.Character.TradeWindowSlot = new ulong[20];
                targetClient.Character.eventSelectExecCode = 0;
            }

            Router.Send(eventEnd, client);
            client.Character.TradeWindowSlot = new ulong[20];
            client.Character.eventSelectExecCode = 0;
        }
    }
}
