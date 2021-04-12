using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvWantedListMember : PacketResponse
    {
        public RecvWantedListMember()
            : base((ushort)AreaPacketId.recv_wanted_list_member, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0x32); // cmp to 0x32 = 50

            int numEntries = 0x32;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("soulname", 49);
                res.WriteInt32(0);
                res.WriteFixedString("charaname", 91);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0); // bool
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);

                res.WriteByte(0);

                res.WriteByte(0);

                res.WriteInt32(0);

                res.WriteInt64(0);
            }

            res.WriteInt32(0);
            return res;
        }
    }
}
