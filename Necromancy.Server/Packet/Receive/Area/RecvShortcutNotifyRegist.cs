using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShortcutNotifyRegist : PacketResponse
    {
        public RecvShortcutNotifyRegist()
            : base((ushort)AreaPacketId.recv_shortcut_notify_regist, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteFixedString("", 0x10);
            return res;
        }
    }
}
