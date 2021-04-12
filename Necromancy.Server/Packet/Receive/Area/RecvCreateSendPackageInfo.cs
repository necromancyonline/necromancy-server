using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCreateSendPackageInfo : PacketResponse
    {
        public RecvCreateSendPackageInfo()
            : base((ushort)AreaPacketId.recv_create_send_pacakge_info, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
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
            for (int i = 0; i < 3; i++)
            {
                res.WriteByte(0); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteInt64(0);
            return res;
        }
    }
}
