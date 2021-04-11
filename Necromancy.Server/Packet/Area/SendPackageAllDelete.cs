using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_package_all_delete : ClientHandler
    {
        public send_package_all_delete(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_package_all_delete;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_package_all_delete_r, res, ServerType.Area);
        }
    }
}
