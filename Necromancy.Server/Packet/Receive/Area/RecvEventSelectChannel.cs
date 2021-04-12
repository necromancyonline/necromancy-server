using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSelectChannel : PacketResponse
    {
        public RecvEventSelectChannel()
            : base((ushort)AreaPacketId.recv_event_select_channel, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteByte(0);

            int numEntries = 0x80;
            for (int i = 0; i < numEntries; i++)

            {
                res.WriteInt32(0);
                res.WriteFixedString("ToBefound", 0x61);
                res.WriteByte(0); // Bool
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }

            res.WriteByte(0);
            return res;
        }
    }
}
