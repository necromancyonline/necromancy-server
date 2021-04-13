using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventShowBoardEnd : PacketResponse
    {
        public RecvEventShowBoardEnd()
            : base((ushort)AreaPacketId.recv_event_show_board_end, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();

            return res;
        }
    }
}
