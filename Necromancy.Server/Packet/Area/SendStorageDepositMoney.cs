using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{

    public class SendStorageDepositMoney : ClientHandler
    {
        public SendStorageDepositMoney(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_storage_deposit_money;

        public override void Handle(NecClient client, NecPacket packet)
        {

            ulong depositeGold = packet.data.ReadUInt64();

        IBuffer res = BufferProvider.Provide();
        res.WriteInt32(0);  // 0 to work
        router.Send(client, (ushort)AreaPacketId.recv_storage_deposit_money_r, res, ServerType.Area);

        client.character.adventureBagGold -= depositeGold; //Updates your Character.AdventureBagGold
        client.soul.warehouseGold += depositeGold; //Updates your Soul.warehouseGold

        IBuffer res2 = BufferProvider.Provide();
        res2.WriteUInt64(client.character.adventureBagGold); // Sets your Adventure Bag Gold
        router.Send(client, (ushort) AreaPacketId.recv_self_money_notify, res2, ServerType.Area);

        }
    }
}
