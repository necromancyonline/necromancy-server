using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_summon_cast_cancel_notify : PacketResponse
    {
        public recv_soul_partner_summon_cast_cancel_notify()
            : base((ushort) AreaPacketId.recv_soul_partner_summon_cast_cancel_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No Structure

            return res;
        }
    }
}
