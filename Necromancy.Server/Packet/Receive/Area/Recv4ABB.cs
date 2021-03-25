using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv4ABB : PacketResponse
    {
        private readonly uint _unknown1;
        private readonly int _unknown2;
        private readonly int _unknown3;

        public Recv4ABB(uint unknown1, int unknown2, int unknown3)
            : base((ushort) AreaPacketId.recv_event_end, ServerType.Area)
        {
            _unknown1 = unknown1;
            _unknown2 = unknown2;
            _unknown3 = unknown3;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(0);
            res.WriteUInt16(0);

            return res;
        }
    }
}
