using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_sell_check : ClientHandler
    {
        public send_shop_sell_check(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_shop_sell_check;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zone = (ItemZoneType)packet.Data.ReadByte();
            byte bag = packet.Data.ReadByte();
            short slot = packet.Data.ReadInt16();
            ulong saleGold = packet.Data.ReadUInt64(); //irrelevant, check sale price server side
            byte quantity = packet.Data.ReadByte();

            //TODO

            ItemService itemService = new ItemService(client.Character);
            ItemInstance itemInstance = itemService.GetIdentifiedItem(new ItemLocation(zone, bag, slot));

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_shop_sell_check_r, res, ServerType.Area);


            res = BufferProvider.Provide();
            res.WriteUInt64(itemInstance.InstanceID); // id?
            res.WriteInt64(Util.GetRandomNumber(1000,25000)); // price?
            res.WriteInt64(Util.GetRandomNumber(100, 2500)); // identify?
            res.WriteInt64(Util.GetRandomNumber(100, 2500)); // curse?
            res.WriteInt64(Util.GetRandomNumber(1000, 2500)); // repair?
            Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_item_sell_price, res, ServerType.Area);
        }

    }
}
