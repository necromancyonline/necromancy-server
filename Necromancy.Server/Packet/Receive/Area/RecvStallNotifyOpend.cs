using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvStallNotifyOpend : PacketResponse
    {
        public RecvStallNotifyOpend()
            : base((ushort)AreaPacketId.recv_stall_notify_opend, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString("ToBeFound"); // find max size 
            int numEntries = 5;
            res.WriteInt32(numEntries); //less than or equal to 5
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt32(0);

                res.WriteByte(0); //Bool
            }

            return res;
        }
    }
}
