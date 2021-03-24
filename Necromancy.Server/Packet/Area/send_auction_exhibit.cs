using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;
using static Necromancy.Server.Systems.Item.ItemService;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_exhibit : ClientHandler
    {
        public send_auction_exhibit(NecServer server) : base(server)  { }
        public override ushort Id => (ushort) AreaPacketId.send_auction_exhibit;
        public override void Handle(NecClient client, NecPacket packet)
        {
            byte exhibitSlot = packet.Data.ReadByte();
            ItemZoneType zone = (ItemZoneType)packet.Data.ReadByte();
            byte bag = packet.Data.ReadByte();
            short slot = packet.Data.ReadInt16();
            byte quantity = packet.Data.ReadByte();
            int time = packet.Data.ReadInt32(); //0:4hours 1:8 hours 2:16 hours 3:24 hours
            ulong minBid = packet.Data.ReadUInt64();
            ulong buyoutPrice = packet.Data.ReadUInt64();
            string comment = packet.Data.ReadCString();

            ItemLocation fromLoc = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.Character);
            ulong instanceId = 0;
            int auctionError = 0;
            try
            {
                MoveResult moveResult = itemService.Exhibit(fromLoc, exhibitSlot, quantity, time, minBid, buyoutPrice, comment);
                instanceId = moveResult.DestItem.InstanceID;
                List<PacketResponse> responses = itemService.GetMoveResponses(client, moveResult);
                Router.Send(client, responses);                
            }
            catch (AuctionException e) { auctionError = (int)e.Type; }            

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(auctionError); //error check.
            res.WriteInt32((int) buyoutPrice); //unknown
            res.WriteUInt64(instanceId); //unknown
            Router.Send(client.Map, (ushort)AreaPacketId.recv_auction_exhibit_r, res, ServerType.Area);
        }        
    }
}
