using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendShopSellCheck : ClientHandler
    {
        public SendShopSellCheck(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_shop_sell_check;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zone = (ItemZoneType)packet.data.ReadByte();
            byte bag = packet.data.ReadByte();
            short slot = packet.data.ReadInt16();
            ulong saleGold = packet.data.ReadUInt64(); //irrelevant, check sale price server side
            byte quantity = packet.data.ReadByte();

            //TODO

            ItemService itemService = new ItemService(client.character);
            ItemInstance itemInstance = itemService.GetIdentifiedItem(new ItemLocation(zone, bag, slot));

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_shop_sell_check_r, res, ServerType.Area);


            res = BufferProvider.Provide();
            res.WriteUInt64(itemInstance.instanceId); // id?
            res.WriteInt64(Util.GetRandomNumber(1000, 25000)); // price?
            res.WriteInt64(Util.GetRandomNumber(100, 2500)); // identify?
            res.WriteInt64(Util.GetRandomNumber(100, 2500)); // curse?
            res.WriteInt64(Util.GetRandomNumber(1000, 2500)); // repair?
            router.Send(client, (ushort)AreaPacketId.recv_shop_notify_item_sell_price, res, ServerType.Area);
        }
    }
}
