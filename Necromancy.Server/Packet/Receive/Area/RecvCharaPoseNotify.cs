using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaPoseNotify : PacketResponse
    {
        public RecvCharaPoseNotify()
            : base((ushort)AreaPacketId.recv_chara_pose_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // character id
            res.WriteInt32(0); // pose id
            return res;
        }
    }
}
