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
            NecClient targetClient = null;
            if (client.Character.eventSelectExecCode != 0)
                targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            RecvTradeRevert tradeRevert = new RecvTradeRevert();
            Router.Send(tradeRevert, client);
            //client.Character.TradeWindowSlot = new ulong[20];

            if (targetClient != null)
            {
                RecvTradeNotifyReverted notifyReverted = new RecvTradeNotifyReverted();
                Router.Send(notifyReverted, targetClient);
                //targetClient.Character.TradeWindowSlot = new ulong[20];
            }
            //client.Character.eventSelectExecCode = 0;
        }
    }
}
