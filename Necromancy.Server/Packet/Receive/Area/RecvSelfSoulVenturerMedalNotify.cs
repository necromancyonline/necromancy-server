using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_self_soul_venturer_medal_notify : PacketResponse
    {
        public recv_self_soul_venturer_medal_notify()
            : base((ushort) AreaPacketId.recv_self_soul_venturer_medal_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("What");
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
