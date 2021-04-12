using System.Numerics;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyMapLink : PacketResponse
    {
        private readonly int _color;
        private readonly byte _heading;
        private readonly uint _instanceId;
        private readonly Vector3 _mapPos;
        private readonly int _offset;
        private readonly int _width;

        public RecvDataNotifyMapLink(uint instanceId, Vector3 mapPos, int offset, int width,
            int color, byte heading)
            : base((ushort)AreaPacketId.recv_data_notify_maplink, ServerType.Area)
        {
            _instanceId = instanceId;
            _mapPos = mapPos;
            _offset = offset = 0;
            _width = width;
            _color = color;
            _heading = heading;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide(); // it's the aura portal for map
            res.WriteUInt32(_instanceId); // Unique ID

            res.WriteFloat(_mapPos.X); //x
            res.WriteFloat(_mapPos.Y); //y
            res.WriteFloat(_mapPos.Z); //z
            res.WriteByte(_heading); //

            res.WriteFloat(_width); // Width
            res.WriteFloat(_offset); // distance away from map coords for particle effects

            res.WriteInt32(_color); // Aura color 0=blue 1=gold 2=white 3=red 4=purple 5=black  0 to 5, crash above 5
            return res;
        }
    }
}
