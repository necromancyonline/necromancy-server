using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_deregist_search_item_cond : ClientHandler
    {
        public send_auction_deregist_search_item_cond(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_deregist_search_item_cond;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte preset = packet.Data.ReadByte(); //Provides the preset number to delete.

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_auction_deregist_search_item_cond_r, res, ServerType.Area);
        }
    }
}
