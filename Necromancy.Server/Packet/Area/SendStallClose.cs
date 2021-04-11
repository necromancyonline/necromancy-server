using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendStallClose : ClientHandler
    {
        public SendStallClose(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_stall_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            router.Send(client, (ushort) AreaPacketId.recv_stall_close_r, res, ServerType.Area);

            SendStallNotifyClosed(client);
        }

        private void SendStallNotifyClosed(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.character.instanceId);

            router.Send(client.map, (ushort)AreaPacketId.recv_stall_notify_closed, res, ServerType.Area);
        }
    }
}
