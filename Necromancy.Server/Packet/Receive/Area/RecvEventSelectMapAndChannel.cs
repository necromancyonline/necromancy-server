using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSelectMapAndChannel : PacketResponse
    {
        public RecvEventSelectMapAndChannel()
            : base((ushort)AreaPacketId.recv_event_select_map_and_channel, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x20;
            res.WriteInt32(numEntries);
            for (int i = 0; i < 0x20; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteByte(0);
                for (int j = 0; j < 0x80; j++)
                {
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x61);
                    res.WriteByte(0); //bool
                    res.WriteInt16(0);
                    res.WriteInt16(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                }

                res.WriteByte(0);
            }

            return res;
        }
    }
}
