using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvObjectPointMoveNotify : PacketResponse
    {
        public RecvObjectPointMoveNotify()
            : base((ushort)AreaPacketId.recv_object_point_move_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFloat(0); //x
            res.WriteFloat(0); //y
            res.WriteFloat(0); //z
            res.WriteByte(0); //view offset
            res.WriteByte(0);
            return res;
        }
    }
}
