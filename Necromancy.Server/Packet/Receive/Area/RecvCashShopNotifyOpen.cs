using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShopNotifyOpen : PacketResponse
    {
        public RecvCashShopNotifyOpen()
            : base((ushort)AreaPacketId.recv_cash_shop_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            int numEntries = 0xA;
            res.WriteInt32(numEntries); // less than or equal to 0xA
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x19);
            }

            numEntries = 0x64;
            res.WriteInt32(numEntries); //less than or equal to 0x64
            res.WriteByte(0);
            res.WriteFixedString("", 0x1F);
            return res;
        }
    }
}
