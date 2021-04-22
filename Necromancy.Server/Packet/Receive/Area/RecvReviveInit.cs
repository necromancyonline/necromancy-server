using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvReviveInit : PacketResponse
    {
        public RecvReviveInit()
            : base((ushort)AreaPacketId.recv_revive_init_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//One of these represents the cost of RG/SC it cost to revive.
            res.WriteInt32(50);//Your current station cash (we should name these nec coins.)
            res.WriteInt32(25);//Cost of station cash to revive.
            return res;
        }
    }
}
