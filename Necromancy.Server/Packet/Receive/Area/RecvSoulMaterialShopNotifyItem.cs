using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulMaterialShopNotifyItem : PacketResponse
    {
        public RecvSoulMaterialShopNotifyItem()
            : base((ushort)AreaPacketId.recv_soulmaterial_shop_notify_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);

            int numEntries = 0x10; // <=0x10
            res.WriteInt32(numEntries); //// <=0x10
            for (int i = 0; i < numEntries; i++) res.WriteByte(0);

            int numEntries2 = 0x4; // <=0x4
            for (int i = 0; i < numEntries2; i++) res.WriteInt32(0);
            return res;
        }
    }
}
