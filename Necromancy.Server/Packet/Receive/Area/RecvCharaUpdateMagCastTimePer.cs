using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateMagCastTimePer : PacketResponse
    {
        public RecvCharaUpdateMagCastTimePer()
            : base((ushort)AreaPacketId.recv_chara_update_mag_cast_time_per, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteFloat(0); // Probably a % value
            return res;
        }
    }
}
