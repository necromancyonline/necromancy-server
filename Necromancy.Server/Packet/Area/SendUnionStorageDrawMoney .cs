using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_union_storage_draw_money : ClientHandler
    {
        public send_union_storage_draw_money(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_union_storage_draw_money;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte unknown = packet.Data.ReadByte();
            ulong WithdrawGold = packet.Data.ReadUInt64();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_union_storage_draw_money_r, res, ServerType.Area);

            //To-Do,  make a variable to track union gold
            client.Character.AdventureBagGold += WithdrawGold; //Updates your Character.AdventureBagGold
            client.Soul.WarehouseGold -= WithdrawGold; //Updates your Soul.warehouseGold

            res = BufferProvider.Provide();
            res.WriteUInt64(client.Character.AdventureBagGold); // Sets your Adventure Bag Gold
            Router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteByte(unknown);
            res.WriteUInt64(client.Soul.WarehouseGold/*client.Union.GeneralSafeGold*/);
            Router.Send(client.Union.UnionMembers, (ushort)AreaPacketId.recv_event_union_storage_update_money, res, ServerType.Area);

        }
    }
}
