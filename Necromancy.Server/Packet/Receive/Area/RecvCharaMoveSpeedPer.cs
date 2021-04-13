using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaMoveSpeedPer : PacketResponse
    {
        public RecvCharaMoveSpeedPer()
            : base((ushort)AreaPacketId.recv_chara_move_speed_per, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
