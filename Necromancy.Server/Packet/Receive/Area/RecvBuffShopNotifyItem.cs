using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBuffShopNotifyItem : PacketResponse
    {
        public RecvBuffShopNotifyItem()
            : base((ushort)AreaPacketId.recv_buff_shop_notify_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteCString(""); // Length 0x3D
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
