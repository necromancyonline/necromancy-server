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
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_trade_abort_r, res, ServerType.Area);

            RecvTradeNotifyAborted notifyAborted = new RecvTradeNotifyAborted();
            Router.Send(notifyAborted, Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode));

            RecvEventEnd eventEnd = new RecvEventEnd(0);
            Router.Send(eventEnd, client);
            Router.Send(eventEnd, Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode));

            Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode).Character.eventSelectExecCode = 0;
            client.Character.eventSelectExecCode = 0;
        }
    }
}
