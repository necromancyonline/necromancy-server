using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPremiumServiceNotifyAttach : PacketResponse
    {
        public RecvPremiumServiceNotifyAttach()
            : base((ushort)AreaPacketId.recv_premium_service_notify_attach, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
