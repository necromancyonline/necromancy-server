using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendCashShopOpenByMenu : ClientHandler
    {
        public SendCashShopOpenByMenu(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_cash_shop_open_by_menu;

        public override void Handle(NecClient client, NecPacket packet)
        {
            //res.WriteUInt32(0);
            //res.WriteByte(1);//Bool

            //Router.Send(client, (ushort) AreaPacketId.recv_cash_shop_open_by_menu, res, ServerType.Area); //toDo  Find the OpCode for this recv.

            IBuffer res = BufferProvider.Provide();

            res.WriteInt16(0b1111); //mode flag
            res.WriteByte(0); //item number
            res.WriteInt32(100); //cash
            int numEntries = 1;
            res.WriteInt32(numEntries);// less than or equal to 0xA
            for (int i = 0; i < numEntries; i++) //tabs number
            {
                res.WriteByte((byte)i);
                res.WriteInt32(i);
                res.WriteFixedString($"test{i}", 0x19);
            }
            numEntries = 1;
            res.WriteInt32(numEntries);//less than or equal to 0x64
            for (int i = 0; i < numEntries; i++)  // filters number
            {
                res.WriteByte((byte)i);
                res.WriteFixedString("hello", 0x1F);
            }

            //Router.Send(client, (ushort) AreaPacketId.recv_cash_shop_notify_open, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt16(1); //mode flag
            res.WriteByte(1); //item number
            res.WriteInt32(125); //cash
            router.Send(client, (ushort)AreaPacketId.recv_cash_shop2_notify_open, res, ServerType.Area);


        }
    }
}
