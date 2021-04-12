using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvObjectHpPerUpdateNotify : PacketResponse
    {
        private readonly uint _instanceId;
        private float _perHp;

        public RecvObjectHpPerUpdateNotify(uint instanceId, float perHp)
            : base((ushort)AreaPacketId.recv_object_hp_per_update_notify, ServerType.Area)
        {
            _instanceId = instanceId;
            _perHp = perHp;
        }

        protected override IBuffer ToBuffer()
        {
            if (_perHp < 0) _perHp = 0;

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteUInt32(_instanceId);
            res4.WriteByte(
                (byte)_perHp); // % hp remaining of target.  need to store current NPC HP and OD as variables to "attack" them
            return res4;
        }
    }
}
