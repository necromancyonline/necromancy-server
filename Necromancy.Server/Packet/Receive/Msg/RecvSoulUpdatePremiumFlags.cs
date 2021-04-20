using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvSoulUpdatePremiumFlags : PacketResponse
    {
        public RecvSoulUpdatePremiumFlags()
            : base((ushort)MsgPacketId.recv_soul_update_premium_flags, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            return res;
        }
    }
}
