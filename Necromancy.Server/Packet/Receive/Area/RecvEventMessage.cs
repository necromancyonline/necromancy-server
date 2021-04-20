using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventMessage : PacketResponse
    {
        private readonly string _message;
        private readonly int _unknown;

        public RecvEventMessage(int unknown, string message)
            : base((ushort)AreaPacketId.recv_event_message, ServerType.Area)
        {
            _unknown = unknown;
            _message = message;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_unknown);
            res.WriteCString(_message);

            return res;
        }
    }
}
