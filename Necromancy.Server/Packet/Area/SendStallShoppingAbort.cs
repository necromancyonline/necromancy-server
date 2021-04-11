using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_stall_shopping_abort : ClientHandler
    {
        public send_stall_shopping_abort(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_stall_shopping_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_stall_shopping_abort_r, res, ServerType.Area);

            SendStallShoppingNotifyAborted(client);
        }

        private void SendStallShoppingNotifyAborted(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            Router.Send(client, (ushort)AreaPacketId.recv_stall_shopping_notify_aborted, res, ServerType.Area);
        }
    }
}
