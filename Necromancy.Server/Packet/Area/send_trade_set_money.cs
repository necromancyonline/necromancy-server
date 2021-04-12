using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_set_money : ClientHandler
    {
        public send_trade_set_money(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_set_money;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = null;
            if (client.Character.eventSelectExecCode != 0)
                targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            ulong myGoldOffer = packet.Data.ReadUInt64();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check.  must be 0 to succeed
            Router.Send(client, (ushort) AreaPacketId.recv_trade_set_money_r, res, ServerType.Area);//ToDo add money to a place where we can exchange it if added to a trade

            if (targetClient != null)
            {
                RecvTradeNotifyMoney notifyMoney = new RecvTradeNotifyMoney(myGoldOffer);
                Router.Send(notifyMoney, targetClient);
            }
        }

    }
}
