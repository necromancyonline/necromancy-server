using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaArrangeNotifyOpen : PacketResponse
    {
        public RecvCharaArrangeNotifyOpen()
            : base((ushort) AreaPacketId.recv_chara_arrange_notify_open, ServerType.Area)
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
