using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSixthSenseTrapNotify : PacketResponse
    {
        public RecvSixthSenseTrapNotify()
            : base((ushort)AreaPacketId.recv_sixthsense_trap_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            return res;
        }
    }
}
