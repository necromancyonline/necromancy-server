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
    public class send_auction_cancel_exhibit : ClientHandler
    {
        public send_auction_cancel_exhibit(NecServer server) : base(server)  {  }
        public override ushort Id => (ushort) AreaPacketId.send_auction_cancel_exhibit;
        public override void Handle(NecClient client, NecPacket packet)
        {
            byte slot = packet.Data.ReadByte();
            ItemService itemService = new ItemService(client.Character);

            int auctionError = 0;
            try
            {
                MoveResult moveResult = itemService.CancelExhibit(slot);
                List<PacketResponse> responses = itemService.GetMoveResponses(client, moveResult);                
                Router.Send(client, responses); //TODO figure out if you can get a popup for this and the winning auction
            }
            catch (AuctionException e) { auctionError = (int)e.Type; }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(auctionError);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_cancel_exhibit_r, res, ServerType.Area);
        }
    }
}
