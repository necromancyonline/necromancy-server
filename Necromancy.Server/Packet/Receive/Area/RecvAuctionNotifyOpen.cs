using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvAuctionNotifyOpen : PacketResponse
    {
        public RecvAuctionNotifyOpen()
            : base((ushort)AreaPacketId.recv_auction_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0xF;
            res.WriteInt32(numEntries); //Less than or equal to 0xF

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteFixedString("soulname", 49);
                res.WriteByte(1);
                res.WriteFixedString("ToBeFound", 385);
                res.WriteInt16(0);
                res.WriteInt32(0);

                res.WriteInt64(0);
                res.WriteInt32(0);
            }

            numEntries = 0xE;
            res.WriteInt32(numEntries); //Less than or equal to 0xE

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteFixedString("soulname", 49);
                res.WriteByte(1);
                res.WriteFixedString("ToBeFound", 385);
                res.WriteInt16(0);
                res.WriteInt32(0);

                res.WriteInt64(0);
                res.WriteInt32(0);
            }

            numEntries = 0x8;
            res.WriteInt32(numEntries); //Less than or equal to 0x8

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteFixedString("fs0x49", 0x49);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt64(0);
                res.WriteByte(0);
                res.WriteByte(0); //Bool

                res.WriteByte(0); //These are 3 separate bytes or a fixed string of 3 characters.
                res.WriteByte(0); //
                res.WriteByte(0); //

                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteFixedString("fs0xC1", 0xC1); //Fixed string of 0xC1 or 0xC1 bytes.
                res.WriteByte(0);
                res.WriteByte(0);
            }

            numEntries = 0x8;
            res.WriteInt32(numEntries); //Less than or equal to 0x8

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteFixedString("fs0x49V2", 0x49);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt64(0);
                res.WriteByte(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteFixedString("fs0xC1V2", 0xC1); //Fixed string of 0xC1 or 0xC1 bytes.
                res.WriteByte(0);
            }

            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
