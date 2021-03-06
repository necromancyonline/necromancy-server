using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerNotifyExpeditionEnd : PacketResponse
    {
        public RecvSoulPartnerNotifyExpeditionEnd()
            : base((ushort)AreaPacketId.recv_soul_partner_notify_expedition_end, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteCString("What");
            return res;
        }
    }
}
