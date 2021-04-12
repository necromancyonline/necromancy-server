using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendStorageDrawMoney : ClientHandler
    {
        public SendStorageDrawMoney(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_storage_draw_money;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ulong withdrawGold = packet.data.ReadUInt64();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_storage_drawmoney, res, ServerType.Area);

            client.character.adventureBagGold += withdrawGold; //Updates your Character.AdventureBagGold
            client.soul.warehouseGold -= withdrawGold; //Updates your Soul.warehouseGold

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt64(client.character.adventureBagGold); // Sets your Adventure Bag Gold
            router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res2, ServerType.Area);
        }
    }
}
