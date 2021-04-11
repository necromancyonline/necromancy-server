using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendUnionStorageDrawMoney : ClientHandler
    {
        public SendUnionStorageDrawMoney(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_union_storage_draw_money;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte unknown = packet.data.ReadByte();
            ulong withdrawGold = packet.data.ReadUInt64();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) AreaPacketId.recv_union_storage_draw_money_r, res, ServerType.Area);

            //To-Do,  make a variable to track union gold
            client.character.adventureBagGold += withdrawGold; //Updates your Character.AdventureBagGold
            client.soul.warehouseGold -= withdrawGold; //Updates your Soul.warehouseGold

            res = BufferProvider.Provide();
            res.WriteUInt64(client.character.adventureBagGold); // Sets your Adventure Bag Gold
            router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteByte(unknown);
            res.WriteUInt64(client.soul.warehouseGold/*client.Union.GeneralSafeGold*/);
            router.Send(client.union.unionMembers, (ushort)AreaPacketId.recv_event_union_storage_update_money, res, ServerType.Area);

        }
    }
}
