using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendStallSetItemPrice : ClientHandler
    {
        public SendStallSetItemPrice(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_stall_set_item_price;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check
                /*
                   STALL	1	You have not listed any items
                   STALL	2	Item has already been listed
                   STALL	3	You may not list the equipped items
                   STALL	4	You may not list items that have not been identified
                   STALL	5	You sold %3%d %2%s to %1%s
                   STALL	6	Shop opened
                   STALL	7	Shop cancelled
                   STALL	8	You have reached the maximum number of items you can list
                   STALL	9	The item you are listing has been stolen
                   STALL	10	Not enough items available in inventory
                   STALL	11	That item has sold out
                   STALL	12	You do not have enough gold
                   STALL	13	No space available in inventory
                   STALL	14	You have not set a name for your shop
                   STALL	15	You may not list a bag currently in use
                   STALL	16	You may not open a shop in soul form
                   STALL	17	Unable to open shop
                   STALL	18	This item may not be listed in a shop
                   STALL	19	You may not equip items you have listed in your shop
                   STALL	20	You have reached the maximum number of items you can purchase
                   STALL	21	You may not use items while your shop is open
                   STALL	22	You may not list any more items
                   STALL	-1600	Your shop value has reached its maximum value
                   STALL	-1601	You are unable to process transactions at your shop right now
                   STALL	-1602	You have no shops open
                   STALL	-1603	Item does not exist
                   STALL	-1604	Incorrect value for the number of items
                   STALL	-1605	No space available in inventory
                   STALL	-1606	You do not have enough gold
                   STALL	-1607	Unable to open the shop here
                   STALL	-1701	Unable to use this name for your shop
                   STALL	-215	This does not belong to you
                   STALL	-3002	Unable to buy or sell in personal shops during an event
                   STALL	GENERIC	Shop error
                */

            router.Send(client, (ushort) AreaPacketId.recv_stall_set_item_price_r, res, ServerType.Area);
        }
    }
}
