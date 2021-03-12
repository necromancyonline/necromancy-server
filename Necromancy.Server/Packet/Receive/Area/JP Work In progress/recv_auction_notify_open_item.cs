using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_auction_notify_open_item : PacketResponse
    {
        public recv_auction_notify_open_item()
            : base((ushort) AreaPacketId.recv_auction_notify_open_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0x2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(numEntries); //less than 0x64
            for (int k = 0; k < numEntries; k++)
            {
                //sub_507480
                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteFixedString("Xeno", 0x31);
                res.WriteByte(0);
                res.WriteFixedString("Xeno", 0x181);
                res.WriteInt16(0);
                res.WriteInt32(0);
            }
            return res;
        }
    }
}
