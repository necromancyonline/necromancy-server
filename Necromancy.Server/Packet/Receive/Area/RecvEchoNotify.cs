using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEchoNotify : PacketResponse
    {
        public RecvEchoNotify()
            : base((ushort)AreaPacketId.recv_echo_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            int numEntries = 0x4E20;
            res.WriteInt32(numEntries); //less than or equal to 0x4E20
            for (int i = 0; i < numEntries; i++)
                res.WriteByte(0);
            return res;
        }
    }
}
