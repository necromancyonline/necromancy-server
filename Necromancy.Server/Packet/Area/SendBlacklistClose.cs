using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendBlacklistClose : ClientHandler
    {
        public SendBlacklistClose(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_blacklist_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();


            //  there is no recv
        }
    }
}
