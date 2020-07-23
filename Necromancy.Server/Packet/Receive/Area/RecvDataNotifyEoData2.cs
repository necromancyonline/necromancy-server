using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyEoData2 : PacketResponse
    {
        public RecvDataNotifyEoData2()
            : base((ushort) AreaPacketId.recv_data_notify_eo_data2, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteFloat(0);

            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteFloat(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);
            return res;
        }
    }
}