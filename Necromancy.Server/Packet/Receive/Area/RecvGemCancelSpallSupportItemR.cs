using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGemCancelSpallSupportItemR : PacketResponse
    {
        public RecvGemCancelSpallSupportItemR()
            : base((ushort) AreaPacketId.recv_gem_cancel_spall_support_item_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            return res;
        }
    }
}
