using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendTradeSetMoney : ClientHandler
    {
        public SendTradeSetMoney(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_trade_set_money;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = null;
            if (client.character.eventSelectExecCode != 0)
                targetClient = server.clients.GetByCharacterInstanceId((uint)client.character.eventSelectExecCode);

            ulong myGoldOffer = packet.data.ReadUInt64();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check.  must be 0 to succeed
            router.Send(client, (ushort) AreaPacketId.recv_trade_set_money_r, res, ServerType.Area);//ToDo add money to a place where we can exchange it if added to a trade

            if (targetClient != null)
            {
                RecvTradeNotifyMoney notifyMoney = new RecvTradeNotifyMoney(myGoldOffer);
                router.Send(notifyMoney, targetClient);
            }
        }

    }
}
