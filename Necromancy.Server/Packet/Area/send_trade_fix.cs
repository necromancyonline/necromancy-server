using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_fix : ClientHandler
    {
        public send_trade_fix(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_fix;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = null;
            if (client.Character.eventSelectExecCode != 0)
                targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check?  to be tested
            Router.Send(client, (ushort) AreaPacketId.recv_trade_fix_r, res, ServerType.Area);

            if (targetClient != null)
            {
                RecvTradeNotifyFixed notifyFixed = new RecvTradeNotifyFixed();
                Router.Send(notifyFixed, targetClient);
            }

            RecvEventEnd eventEnd = new RecvEventEnd(0);
            Router.Send(eventEnd, client);
            
            client.Character.eventSelectExecCode = 0;
        }
    }
}
