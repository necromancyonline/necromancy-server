using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_roguemap_entry_notify_party_member_entry : PacketResponse
    {
        public recv_roguemap_entry_notify_party_member_entry()
            : base((ushort) AreaPacketId.recv_roguemap_entry_notify_party_member_entry, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
