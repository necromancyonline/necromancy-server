using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfSoulRankNotify : PacketResponse
    {
        public RecvSelfSoulRankNotify()
            : base((ushort)AreaPacketId.recv_self_soul_rank_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteByte(0);

            res.WriteByte(0);

            res.WriteByte(0);

            res.WriteByte(0);
            return res;
        }
    }
}
