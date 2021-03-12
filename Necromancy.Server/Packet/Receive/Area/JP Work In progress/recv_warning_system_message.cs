using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_warning_system_message : PacketResponse
    {
        public recv_warning_system_message()
            : base((ushort) AreaPacketId.recv_warning_system_message, ServerType.Area)
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
