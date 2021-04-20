using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventStart : PacketResponse
    {
        private readonly uint _type;
        private readonly byte _unknown;

        public RecvEventStart(uint type, byte unknown)
            : base((ushort)AreaPacketId.recv_event_start, ServerType.Area)
        {
            _type = type;
            _unknown = unknown;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_type); // 0 = normal 1 = cinematic
            res.WriteByte(_unknown);

            return res;
        }
    }
}
