using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPackageAllDelete : ClientHandler
    {
        public SendPackageAllDelete(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_package_all_delete;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            router.Send(client, (ushort) AreaPacketId.recv_package_all_delete_r, res, ServerType.Area);
        }
    }
}
