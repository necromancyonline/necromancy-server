using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_blacklist_close : ClientHandler
    {
        public send_blacklist_close(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_blacklist_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();


            //  there is no recv
        }
    }
}
