using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGetRecvPackage : PacketResponse
    {
        public RecvGetRecvPackage()
            : base((ushort)AreaPacketId.recv_get_recv_package, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0x27; //max is 0x64 but it is too high
            res.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("", 0x31);
                res.WriteFixedString("", 0x5B);
                res.WriteFixedString("", 0x5B);
                res.WriteFixedString("", 0x259);
                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt64(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x49);
                res.WriteFixedString("", 0x49);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x10);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteByte(0); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteByte(0); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteByte(0); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt64(0);
            }

            res.WriteInt32(0);
            return res;
        }
    }
}
