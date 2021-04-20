using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerNotifyAvatar : PacketResponse
    {
        public RecvSoulPartnerNotifyAvatar()
            : base((ushort)AreaPacketId.recv_soul_partner_notify_avatar, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(100001);
            res.WriteByte(1);
            return res;
        }
    }
}
