using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_help_new_remove : ClientHandler
    {
        public send_help_new_remove(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_help_new_remove;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_help_new_remove_r, res, ServerType.Area);
        }

    }
}
