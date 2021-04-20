using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendTempleClose : ClientHandler
    {
        public SendTempleClose(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_temple_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client.map, (ushort)AreaPacketId.recv_temple_close_r, res, ServerType.Area);
            SendTempleNotifyClose(client);
        }

        private void SendTempleNotifyClose(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            router.Send(client.map, (ushort)AreaPacketId.recv_temple_notify_close, res, ServerType.Area, client);
        }
    }
}
