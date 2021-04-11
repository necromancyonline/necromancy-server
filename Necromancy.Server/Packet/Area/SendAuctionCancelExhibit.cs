using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;
using static Necromancy.Server.Systems.Item.ItemService;

namespace Necromancy.Server.Packet.Area
{
    public class SendAuctionCancelExhibit : ClientHandler
    {
        public SendAuctionCancelExhibit(NecServer server) : base(server)  {  }
        public override ushort id => (ushort) AreaPacketId.send_auction_cancel_exhibit;
        public override void Handle(NecClient client, NecPacket packet)
        {
            byte slot = packet.data.ReadByte();
            ItemService itemService = new ItemService(client.character);

            int auctionError = 0;
            try
            {
                MoveResult moveResult = itemService.CancelExhibit(slot);
                List<PacketResponse> responses = itemService.GetMoveResponses(client, moveResult);
                router.Send(client, responses); //TODO figure out if you can get a popup for this and the winning auction
            }
            catch (AuctionException e) { auctionError = (int)e.type; }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(auctionError);
            router.Send(client.map, (ushort) AreaPacketId.recv_auction_cancel_exhibit_r, res, ServerType.Area);
        }
    }
}
