using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateNotifyComment : PacketResponse
    {
        public RecvCharaUpdateNotifyComment()
            : base((ushort)AreaPacketId.recv_chara_update_notify_comment, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(""); // Length 0x181
            return res;
        }
    }
}
