using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaArrangeNotifyParts : PacketResponse
    {
        public RecvCharaArrangeNotifyParts()
            : base((ushort) AreaPacketId.recv_chara_arrange_notify_parts, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteCString("What");
            res.WriteInt32(0);
            res.WriteCString("What");
            return res;
        }
    }
}
