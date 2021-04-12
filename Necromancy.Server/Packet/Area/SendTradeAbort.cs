using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendTradeAbort : ClientHandler
    {
        public SendTradeAbort(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_trade_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = server.clients.GetByCharacterInstanceId((uint)client.character.eventSelectExecCode);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_trade_abort_r, res, ServerType.Area);

            RecvEventEnd eventEnd = new RecvEventEnd(0);
            if (targetClient != null)
            {
                RecvTradeNotifyAborted notifyAborted = new RecvTradeNotifyAborted();
                router.Send(notifyAborted, targetClient);
                router.Send(eventEnd, targetClient);
                targetClient.character.tradeWindowSlot = new ulong[20];
                targetClient.character.eventSelectExecCode = 0;
            }

            router.Send(eventEnd, client);
            client.character.tradeWindowSlot = new ulong[20];
            client.character.eventSelectExecCode = 0;
        }
    }
}
