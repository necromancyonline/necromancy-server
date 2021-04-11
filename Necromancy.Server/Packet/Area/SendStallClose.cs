using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_stall_close : ClientHandler
    {
        public send_stall_close(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_stall_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_stall_close_r, res, ServerType.Area);

            SendStallNotifyClosed(client);
        }

        private void SendStallNotifyClosed(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.Character.InstanceId);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_stall_notify_closed, res, ServerType.Area);
        }
    }
}
