using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaTargetMoveSideSpeedPer : PacketResponse
    {
        public RecvCharaTargetMoveSideSpeedPer()
            : base((ushort)AreaPacketId.recv_chara_target_move_side_speed_per, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteInt16(0); //Percent most likely
            return res;
        }
    }
}
