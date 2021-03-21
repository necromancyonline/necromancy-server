using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfNotifyAbyssSteadPos : PacketResponse
    {
        private float _x;
        private float _y;
        private float _z;
        public RecvCharaBodySelfNotifyAbyssSteadPos(float x, float y, float z)
            : base((ushort) AreaPacketId.recv_charabody_self_notify_abyss_stead_pos, ServerType.Area)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteFloat(_x);
            res.WriteFloat(_y);
            res.WriteFloat(_z);
            return res;
        }
    }
}
