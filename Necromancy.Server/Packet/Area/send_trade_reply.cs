using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_reply : ClientHandler
    {
        public send_trade_reply(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_reply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint myTargetID = packet.Data.ReadUInt32();
            int error = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(error);
            Router.Send(client, (ushort) AreaPacketId.recv_trade_reply_r, res, ServerType.Area);

            RecvTradeNotifyReplied notifyReplied = new RecvTradeNotifyReplied(error);
            Router.Send(notifyReplied, Server.Clients.GetByCharacterInstanceId(myTargetID));

            if(error == 0)//Success condition
            {
                RecvEventStart eventStart = new RecvEventStart(0, 0);
                Router.Send(eventStart, client);
                Router.Send(eventStart, Server.Clients.GetByCharacterInstanceId(myTargetID));

                client.Character.eventSelectExecCode = (int)myTargetID;
                Server.Clients.GetByCharacterInstanceId(myTargetID).Character.eventSelectExecCode = (int)client.Character.InstanceId;
            }
        }
    }
}
