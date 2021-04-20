using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEoNotifyDisappearSchedule : PacketResponse
    {
        private readonly float _disappearTime;
        private readonly uint _instanceId;

        public RecvEoNotifyDisappearSchedule(uint instanceId, float disappearTime)
            : base((ushort)AreaPacketId.recv_eo_notify_disappear_schedule, ServerType.Area)
        {
            _instanceId = instanceId;
            _disappearTime = disappearTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            res.WriteFloat(_disappearTime);
            return res;
        }
    }
}
