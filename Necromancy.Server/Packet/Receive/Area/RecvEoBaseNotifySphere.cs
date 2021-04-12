using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEoBaseNotifySphere : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly int _radius;

        public RecvEoBaseNotifySphere(uint instanceId, int radius)
            : base((ushort)AreaPacketId.recv_eo_base_notify_sphere, ServerType.Area)
        {
            _instanceId = instanceId;
            _radius = radius;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            res.WriteFloat(_radius);
            return res;
        }
    }
}
