using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCharaDrawBonusPoint : PacketResponse
    {
        public RecvCharaDrawBonusPoint()
            : base((ushort)MsgPacketId.recv_chara_draw_bonuspoint_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);
            return res;
        }
    }
}
