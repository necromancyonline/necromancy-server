using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_roguemap_update_monster_defeat_count : PacketResponse
    {
        public recv_roguemap_update_monster_defeat_count()
            : base((ushort) AreaPacketId.recv_roguemap_update_monster_defeat_count, ServerType.Area)
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
