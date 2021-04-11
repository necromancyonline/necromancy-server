using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendTempleCureCurse : ClientHandler
    {
        public SendTempleCureCurse(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_temple_cure_curse;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client.map, (ushort) AreaPacketId.recv_temple_cure_curse_r, res, ServerType.Area);
        }

    }
}
