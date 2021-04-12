using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRandomBoxNotifyOpen : PacketResponse
    {
        public RecvRandomBoxNotifyOpen()
            : base((ushort)AreaPacketId.recv_random_box_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 10;
            res.WriteInt32(numEntries); //less than or equal to 10
            for (int i = 0; i < numEntries; i++) res.WriteInt64(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
