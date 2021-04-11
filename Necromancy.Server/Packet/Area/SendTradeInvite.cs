using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_invite : ClientHandler
    {
        public send_trade_invite(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint myTargetID = packet.Data.ReadUInt32();
            
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);  // error check.  1 auto cancels the trade,  0 "the trade has been presented to %d. Awaiting response"
            Router.Send(client, (ushort) AreaPacketId.recv_trade_invite_r, res, ServerType.Area);

            RecvTradeNotifyInvited notifyInvited = new RecvTradeNotifyInvited(client.Character.InstanceId);
            Router.Send(notifyInvited, Server.Clients.GetByCharacterInstanceId(myTargetID));
        }
    }
}
