using System;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendLogoutCancelRequest : ClientHandler
    {
        public SendLogoutCancelRequest(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_logout_cancel_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.logoutCancelationCheck = true;
            //client.character.characterTask.Logout(DateTime.MinValue, 0);
            IBuffer res = BufferProvider.Provide();
            router.Send(client, (ushort)AreaPacketId.recv_logout_cancel, res, ServerType.Area);


            res = BufferProvider.Provide();
            res.WriteInt32(0); //Result.  0 = success.  failed to cancel = 1
            router.Send(client, (ushort) AreaPacketId.recv_logout_cancel_request_r, res, ServerType.Area);
        }
    }
}
