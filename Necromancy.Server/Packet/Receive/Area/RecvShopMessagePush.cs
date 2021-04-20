using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopMessagePush : PacketResponse
    {
        public RecvShopMessagePush()
            : base((ushort)AreaPacketId.recv_shop_message_push, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(""); //Length is 31-01=30=DEC48
            return res;
        }
    }
}
