using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSystemMessage : PacketResponse
    {
        private readonly string _message;

        public RecvEventSystemMessage(string message)
            : base((ushort)AreaPacketId.recv_event_system_message, ServerType.Area)
        {
            _message = message;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(_message);

            return res;
        }
    }
}
