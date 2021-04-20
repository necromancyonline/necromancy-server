using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGetSendPackage : PacketResponse
    {
        public RecvGetSendPackage()
            : base((ushort)AreaPacketId.recv_get_send_package, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0x1E;
            res.WriteInt32(numEntries); //less than or equal to 0x1E
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
                for (int j = 0; j < 3; j++)
                {
                    res.WriteByte(0); //bool
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                }

                res.WriteInt64(0);
            }

            return res;
        }
    }
}
