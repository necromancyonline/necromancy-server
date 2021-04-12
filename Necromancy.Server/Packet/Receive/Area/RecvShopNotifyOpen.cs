using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopNotifyOpen : PacketResponse
    {
        public RecvShopNotifyOpen()
            : base((ushort)AreaPacketId.recv_shop_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0); //Shop type
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            return res;
        }
    }
}
