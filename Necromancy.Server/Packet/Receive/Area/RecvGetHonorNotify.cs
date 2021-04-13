using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGetHonorNotify : PacketResponse
    {
        public RecvGetHonorNotify()
            : base((ushort)AreaPacketId.recv_get_honor_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); // must be under 0x3E8 // wtf?

            int numEntries = 0x1;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0); // bool		
            }

            return res;
        }
    }
}
