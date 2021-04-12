using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendMapChangeForceR : ClientHandler
    {
        public SendMapChangeForceR(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_map_change_force_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_map_entry_r, res, ServerType.Area);

            //why isn't recv_map_change_force() here??
        }
    }
}
