using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_challengemap_area_start : PacketResponse
    {
        public recv_challengemap_area_start()
            : base((ushort) AreaPacketId.recv_challengemap_area_start, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0x31);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0x5B);
            for (int j = 0; j < 0x3; j++)
            {
                res.WriteInt64(0);
            }

            res.WriteInt32(0);
            return res;
        }
    }
}
