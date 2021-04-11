using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyRegistPartyAutoRecruitR : PacketResponse
    {
        public RecvPartyRegistPartyAutoRecruitR()
            : base((ushort) AreaPacketId.recv_party_regist_party_auto_recruit_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            return res;
        }
    }
}
