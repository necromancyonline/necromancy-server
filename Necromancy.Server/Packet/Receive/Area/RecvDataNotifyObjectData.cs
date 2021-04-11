using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyObjectData : PacketResponse
    {
        private readonly Object _objectData;

        public RecvDataNotifyObjectData(Object objectData)
            : base((ushort) AreaPacketId.recv_data_notify_itemobject_data, ServerType.Area)
        {
            _objectData = objectData;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_objectData.instanceId);
            res.WriteFloat(_objectData.objectCoord.X);
            res.WriteFloat(_objectData.objectCoord.Y);
            res.WriteFloat(_objectData.objectCoord.Z);

            res.WriteFloat(_objectData.triggerCoord.X);
            res.WriteFloat(_objectData.triggerCoord.Y);
            res.WriteFloat(_objectData.triggerCoord.Z);
            res.WriteByte(_objectData.heading);

            res.WriteInt32(_objectData.bitmap1);
            res.WriteInt32(_objectData.unknown1);
            res.WriteInt32(_objectData.unknown2);

            res.WriteInt32(_objectData.bitmap2);

            res.WriteInt32(_objectData.unknown3);
            return res;
        }
    }
}
