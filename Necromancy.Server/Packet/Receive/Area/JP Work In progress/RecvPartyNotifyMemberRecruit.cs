using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_party_notify_member_recruit : PacketResponse
    {
        public recv_party_notify_member_recruit()
            : base((ushort) AreaPacketId.recv_party_notify_member_recruit, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //sub_4942F0
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0xB5);
            //end_sub
            return res;
        }
    }
}
