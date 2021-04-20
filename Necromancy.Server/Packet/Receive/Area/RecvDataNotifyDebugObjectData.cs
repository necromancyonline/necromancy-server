using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyDebugObjectData : PacketResponse
    {
        public RecvDataNotifyDebugObjectData()
            : base((ushort)AreaPacketId.recv_data_notify_debug_object_data, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0x20;
            res.WriteInt32(numEntries); //less than or equal to 0x20
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteFloat(0);
                res.WriteFloat(0);
                res.WriteFloat(0);
            }

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
