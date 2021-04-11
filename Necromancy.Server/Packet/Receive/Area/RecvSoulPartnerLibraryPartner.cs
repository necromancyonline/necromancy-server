using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerLibraryPartner : PacketResponse
    {
        public RecvSoulPartnerLibraryPartner()
            : base((ushort) AreaPacketId.recv_soul_partner_library_partner, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(numEntries); //Less than 0x19

            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                int numEntries2 = 0x7; //must be 0x7
                //495f20
                for (int k = 0; k < numEntries2; k++)
                {
                    res.WriteInt16(0);
                }
                int numEntries3 = 0x5; //must be 0x5
                for (int k = 0; k < numEntries3; k++)
                {
                    res.WriteInt32(0);
                }

            }
            return res;
        }
    }
}
