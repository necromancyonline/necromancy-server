using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSelectExecWinpos : PacketResponse
    {
        public RecvEventSelectExecWinpos()
            : base((ushort)AreaPacketId.recv_event_select_exec_winpos, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(""); //Length 0xC01
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
