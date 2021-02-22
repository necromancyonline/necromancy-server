using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{

    public class send_storage_draw_money : ClientHandler
    {
        public send_storage_draw_money(NecServer server) : base(server)
        {

        }
        public override ushort Id => (ushort)AreaPacketId.send_storage_draw_money;

        public override void Handle(NecClient client, NecPacket packet)
        {

            ulong WithdrawGold = packet.Data.ReadUInt64();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); 
            Router.Send(client, (ushort)AreaPacketId.recv_storage_drawmoney, res, ServerType.Area);

            client.Character.AdventureBagGold += WithdrawGold; //Updates your Character.AdventureBagGold
            client.Soul.WarehouseGold -= WithdrawGold; //Updates your Soul.warehouseGold

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt64(client.Character.AdventureBagGold); // Sets your Adventure Bag Gold
            Router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res2, ServerType.Area);

        }

    }
}
