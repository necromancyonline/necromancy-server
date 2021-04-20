using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartySearchRecruitedMember : PacketResponse
    {
        public RecvPartySearchRecruitedMember()
            : base((ushort)AreaPacketId.recv_party_search_recruited_member_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0x14); // cmp to 0x14 = 20


            int numEntries = 0x14;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("soulname", 49);
                res.WriteFixedString("charaname", 91);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0); // bool
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("ToBeFound", 181);
            }

            return res;
        }
    }
}
