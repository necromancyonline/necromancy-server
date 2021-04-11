using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerSummonExecR : PacketResponse
    {
        public RecvSoulPartnerSummonExecR()
            : base((ushort) AreaPacketId.recv_soul_partner_summon_exec_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(200000002);
            res.WriteInt32(100001);
            return res;
        }
    }
}
