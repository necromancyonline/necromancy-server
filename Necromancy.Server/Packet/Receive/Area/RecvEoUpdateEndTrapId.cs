using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEoUpdateEndTrapId : PacketResponse
    {
        private readonly int _effectId;
        private readonly uint _instanceId;

        public RecvEoUpdateEndTrapId(uint instanceId, int effectId)
            : base((ushort)AreaPacketId.recv_eo_update_end_trapid, ServerType.Area)
        {
            _instanceId = instanceId;
            _effectId = effectId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            res.WriteInt32(_effectId);
            return res;
        }
    }
}
