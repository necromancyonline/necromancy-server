using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_logout_cancel_request : ClientHandler
    {
        public send_logout_cancel_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_logout_cancel_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.Character.characterTask.Logout(DateTime.MinValue, 0);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//Ready to discover

            //Router.Send(client, (ushort) AreaPacketId.recv_logout_cancel_request_r, res, ServerType.Area);
        }
    }
}
