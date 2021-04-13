using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGetThreadAll : PacketResponse
    {
        public RecvGetThreadAll()
            : base((ushort)AreaPacketId.recv_get_thread_all_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0x3E8;
            res.WriteInt32(numEntries); //less than or equal to 0x3E8
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x3D);
                res.WriteInt32(0);
                res.WriteInt16(0);
            }

            return res;
        }
    }
}
