using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvVenturerMedalShopNotifyItem : PacketResponse
    {
        public RecvVenturerMedalShopNotifyItem()
            : base((ushort) AreaPacketId.recv_venturer_medal_shop_notify_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);
            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0x10);
            res.WriteInt32(0);

            numEntries = 0x5; //must be 5
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
            }

            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteByte(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
