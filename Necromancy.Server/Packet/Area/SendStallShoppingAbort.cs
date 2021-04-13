using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendStallShoppingAbort : ClientHandler
    {
        public SendStallShoppingAbort(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_stall_shopping_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            router.Send(client, (ushort)AreaPacketId.recv_stall_shopping_abort_r, res, ServerType.Area);

            SendStallShoppingNotifyAborted(client);
        }

        private void SendStallShoppingNotifyAborted(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            router.Send(client, (ushort)AreaPacketId.recv_stall_shopping_notify_aborted, res, ServerType.Area);
        }
    }
}
