using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaArrangeNotifyClose : PacketResponse
    {
        public RecvCharaArrangeNotifyClose()
            : base((ushort)AreaPacketId.recv_chara_arrange_notify_close, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //no structure
            return res;
        }
    }
}
