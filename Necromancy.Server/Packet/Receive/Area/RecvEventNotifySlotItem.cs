using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventNotifySlotItem : PacketResponse
    {
        public RecvEventNotifySlotItem()
            : base((ushort)AreaPacketId.recv_event_notify_slot_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(10390001);
            res.WriteFixedString("DAGGER", 0x10);
            return res;
        }
    }
}
