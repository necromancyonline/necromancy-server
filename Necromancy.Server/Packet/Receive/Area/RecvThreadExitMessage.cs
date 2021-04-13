using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvThreadExitMessage : PacketResponse
    {
        public RecvThreadExitMessage()
            : base((ushort)AreaPacketId.recv_thread_exit_message, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ToBeFound");

            res.WriteCString("ToBeFound");

            res.WriteInt16(0);
            return res;
        }
    }
}
