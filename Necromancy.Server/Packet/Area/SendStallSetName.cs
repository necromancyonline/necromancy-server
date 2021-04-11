using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_stall_set_name : ClientHandler
    {
        public send_stall_set_name(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_stall_set_name;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_stall_set_name_r, res, ServerType.Area);            
        }
    }
}
