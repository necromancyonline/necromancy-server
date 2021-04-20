using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvWantedJailDrawPoint : PacketResponse
    {
        public RecvWantedJailDrawPoint()
            : base((ushort)AreaPacketId.recv_wanted_jail_draw_point_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0); //bool
            return res;
        }
    }
}
