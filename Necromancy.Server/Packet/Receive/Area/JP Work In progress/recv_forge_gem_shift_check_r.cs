using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_forge_gem_shift_check_r : PacketResponse
    {
        public recv_forge_gem_shift_check_r()
            : base((ushort) AreaPacketId.recv_forge_gem_shift_check_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteByte(0);
            return res;
        }
    }
}