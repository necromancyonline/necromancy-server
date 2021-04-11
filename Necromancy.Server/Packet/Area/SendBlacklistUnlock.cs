using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_blacklist_unlock : ClientHandler
    {
        public send_blacklist_unlock(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_blacklist_unlock;

       public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);




            Router.Send(client, (ushort)AreaPacketId.recv_blacklist_unlock_r, res, ServerType.Area);
        }
    }
}
