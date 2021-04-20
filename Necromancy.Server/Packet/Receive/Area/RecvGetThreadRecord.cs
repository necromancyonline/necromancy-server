using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGetThreadRecord : PacketResponse
    {
        public RecvGetThreadRecord()
            : base((ushort)AreaPacketId.recv_get_thread_record_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0x32;
            res.WriteInt32(numEntries); //less than or equal to 0x32
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt16(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x31);
                res.WriteFixedString("", 0x25);
                res.WriteFixedString("", 0x301);
            }

            return res;
        }
    }
}
