using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfExpNotify : PacketResponse
    {
        public RecvSelfExpNotify()
            : base((ushort)AreaPacketId.recv_self_exp_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteByte(0); //bool
            return res;
        }
    }
}
