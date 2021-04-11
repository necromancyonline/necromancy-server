using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaChangeModelNotify : PacketResponse
    {
        public RecvCharaChangeModelNotify()
            : base((ushort) AreaPacketId.recv_chara_change_model_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
