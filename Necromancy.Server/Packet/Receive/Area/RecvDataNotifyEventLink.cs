using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyEventLink : PacketResponse
    {
        public RecvDataNotifyEventLink()
            : base((ushort)AreaPacketId.recv_data_notify_eventlink, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteByte(0);

            res.WriteFloat(0);
            res.WriteFloat(0);

            res.WriteInt32(0);

            return res;
        }
    }
}
