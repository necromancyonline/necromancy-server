using System.Numerics;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyEoData : PacketResponse
    {
        private readonly int _effectId;
        private readonly uint _instanceId;
        private readonly Vector3 _target;
        private readonly uint _targetInstanceId;
        private readonly int _unknown1;
        private readonly int _unknown2;

        public RecvDataNotifyEoData(uint instanceId, uint targetInstanceId, int effectId, Vector3 target, int unknown1,
            int unknown2)
            : base((ushort)AreaPacketId.recv_data_notify_eo_data, ServerType.Area)
        {
            _instanceId = instanceId;
            _targetInstanceId = targetInstanceId;
            _target = target;
            _effectId = effectId;
            _unknown1 = unknown1;
            _unknown2 = unknown2;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId); // Unique Instance ID of Skill Cast
            res.WriteFloat(_target.X); //Effect Object X
            res.WriteFloat(_target.Y); //Effect Object y
            res.WriteFloat(_target.Z); //Effect Object z    (+100 just so i can see it better for now)

            //orientation related  (Note,  i believe at least 1 of these values must be above 0 for "arrows" to render"
            res.WriteFloat(1); //Rotation Along X Axis if above 0
            res.WriteFloat(1); //Rotation Along Y Axis if above 0
            res.WriteFloat(0); //Rotation Along Z Axis if above 0

            res.WriteInt32(_effectId); // effect id
            res.WriteUInt32(
                _targetInstanceId); //must be set to int32 contents. int myTargetID = packet.Data.ReadInt32();
            res.WriteInt32(_unknown1); //unknown
            res.WriteInt32(_unknown2);
            return res;
        }
    }
}
