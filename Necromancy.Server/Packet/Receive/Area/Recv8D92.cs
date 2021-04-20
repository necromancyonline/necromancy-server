using System.Numerics;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv8D92 : PacketResponse
    {
        private readonly byte _animation;
        private readonly int _castVelocity;
        private readonly uint _instanceId;
        private readonly byte _pose;
        private readonly int _skillId;
        private readonly Vector3 _srcCoord;
        private readonly Vector3 _trgCoord;

        public Recv8D92(Vector3 srcCoord, Vector3 trgCoord, uint instanceId, int skillId, int castVelocity, byte pose,
            byte animation)
            : base((ushort)AreaPacketId.recv_0x8D92, ServerType.Area)
        {
            _instanceId = instanceId;
            _skillId = skillId;
            _castVelocity = castVelocity;
            _pose = pose;
            _animation = animation;
            _srcCoord = srcCoord;
            _trgCoord = trgCoord;
        }

        protected override IBuffer ToBuffer()
        {
            Vector3 moveTo = Vector3.Subtract(_trgCoord, _srcCoord);
            float distance = Vector3.Distance(_srcCoord, _trgCoord);
            float travelTime = distance / _castVelocity;

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId); //Monster ID
            res.WriteFloat(_srcCoord.X);
            res.WriteFloat(_srcCoord.Y);
            res.WriteFloat(_srcCoord.Z + 75);
            res.WriteFloat(moveTo.X); //X per tick
            res.WriteFloat(moveTo.Y); //Y Per tick
            res.WriteFloat(moveTo.Z); //verticalMovementSpeedMultiplier

            res.WriteFloat(1 / travelTime); //movementMultiplier
            res.WriteFloat(travelTime); //Seconds to move

            res.WriteByte(_pose); //MOVEMENT ANIM
            res.WriteByte(_animation); //JUMP & FALLING ANIM
            return res;
        }
    }
}
