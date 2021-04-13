using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSelectExec : PacketResponse
    {
        public RecvEventSelectExec()
            : base((ushort)AreaPacketId.recv_event_select_exec, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ToBeFound"); // find max size
            res.WriteInt32(0);
            return res;
        }
    }
}
