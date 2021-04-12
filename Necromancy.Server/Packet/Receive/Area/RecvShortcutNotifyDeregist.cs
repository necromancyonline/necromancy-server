using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShortcutNotifyDeregist : PacketResponse
    {
        public RecvShortcutNotifyDeregist()
            : base((ushort)AreaPacketId.recv_shortcut_notify_deregist, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
