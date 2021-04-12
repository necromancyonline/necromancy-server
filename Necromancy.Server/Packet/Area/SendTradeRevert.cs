using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendTradeRevert : ClientHandler
    {
        public SendTradeRevert(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_trade_revert;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = null;
            if (client.character.eventSelectExecCode != 0)
                targetClient = server.clients.GetByCharacterInstanceId((uint)client.character.eventSelectExecCode);

            RecvTradeRevert tradeRevert = new RecvTradeRevert();
            router.Send(tradeRevert, client);
            //client.Character.TradeWindowSlot = new ulong[20];

            if (targetClient != null)
            {
                RecvTradeNotifyReverted notifyReverted = new RecvTradeNotifyReverted();
                router.Send(notifyReverted, targetClient);
                //targetClient.Character.TradeWindowSlot = new ulong[20];
            }

            //client.Character.eventSelectExecCode = 0;
        }
    }
}
