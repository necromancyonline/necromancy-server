using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateMagMpCostPer : PacketResponse
    {
        public RecvCharaUpdateMagMpCostPer()
            : base((ushort)AreaPacketId.recv_chara_update_mag_mp_cost_per, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0); //Percentage most likely
            return res;
        }
    }
}
