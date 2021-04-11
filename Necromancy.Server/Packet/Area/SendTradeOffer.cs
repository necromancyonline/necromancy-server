using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_offer : ClientHandler
    {
        public send_trade_offer(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_offer;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = null;
            if (client.Character.eventSelectExecCode != 0)
                targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);



            if (targetClient != null)
            {
                RecvTradeNotifyOfferd notifyOfferd = new RecvTradeNotifyOfferd();
                Router.Send(notifyOfferd, targetClient);
            }
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check?
            Router.Send(client, (ushort)AreaPacketId.recv_trade_offer_r, res, ServerType.Area);
        }

    }
}
