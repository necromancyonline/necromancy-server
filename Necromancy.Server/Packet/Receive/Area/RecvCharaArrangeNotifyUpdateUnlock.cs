using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaArrangeNotifyUpdateUnlock : PacketResponse
    {
        public RecvCharaArrangeNotifyUpdateUnlock()
            : base((ushort)AreaPacketId.recv_chara_arrange_notify_update_unlock, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            for (int i = 0; i < 100; i++) res.WriteInt64(i);
            return res;
        }
    }
}
