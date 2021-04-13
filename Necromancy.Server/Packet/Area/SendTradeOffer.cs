using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendTradeOffer : ClientHandler
    {
        public SendTradeOffer(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_trade_offer;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = null;
            if (client.character.eventSelectExecCode != 0)
                targetClient = server.clients.GetByCharacterInstanceId((uint)client.character.eventSelectExecCode);


            if (targetClient != null)
            {
                RecvTradeNotifyOfferd notifyOfferd = new RecvTradeNotifyOfferd();
                router.Send(notifyOfferd, targetClient);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check?
            router.Send(client, (ushort)AreaPacketId.recv_trade_offer_r, res, ServerType.Area);
        }
    }
}
