using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_gamepot_casino_open : PacketResponse
    {
        public recv_gamepot_casino_open()
            : base((ushort) AreaPacketId.recv_gamepot_casino_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("What");

            return res;
        }
    }
}
