using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRoguemapNotifyRetire : PacketResponse
    {
        public RecvRoguemapNotifyRetire()
            : base((ushort) AreaPacketId.recv_roguemap_notify_retire, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            for (int i = 0; i < 0x4; i++)
            {
                res.WriteInt32(0);
            }
            for (int i = 0; i < 0x4; i++)
            {
                res.WriteInt32(0);
            }
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            //end_sub_496710
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt64(0);
            return res;
        }
    }
}
