using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendStallShoppingStart : ClientHandler
    {
        public SendStallShoppingStart(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_stall_shopping_start;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int stallOwnerCharacterId = packet.data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            router.Send(client, (ushort)AreaPacketId.recv_stall_shopping_start_r, res, ServerType.Area);
        }
    }
}
