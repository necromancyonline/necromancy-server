using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvMonsterStateUpdateNotify : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly int _state;

        public RecvMonsterStateUpdateNotify(uint instanceId, int state)
            : base((ushort)AreaPacketId.recv_monster_state_update_notify, ServerType.Area)
        {
            _instanceId = instanceId;
            _state = state;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            res.WriteInt32(_state);

            return res;
        }
    }
}
