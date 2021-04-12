using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendTradeReply : ClientHandler
    {
        public SendTradeReply(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_trade_reply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint myTargetId = packet.data.ReadUInt32();
            int error = packet.data.ReadInt32();

            NecClient targetClient = server.clients.GetByCharacterInstanceId(myTargetId);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(error);
            router.Send(client, (ushort)AreaPacketId.recv_trade_reply_r, res, ServerType.Area);

            RecvTradeNotifyReplied notifyReplied = new RecvTradeNotifyReplied(error);
            router.Send(notifyReplied, targetClient);

            if (error == 0) //Success condition
            {
                RecvEventStart eventStart = new RecvEventStart(0, 0);
                router.Send(eventStart, client, targetClient);

                client.character.eventSelectExecCode = (int)myTargetId;
                server.clients.GetByCharacterInstanceId(myTargetId).character.eventSelectExecCode = (int)client.character.instanceId;
            }
        }
    }
}
