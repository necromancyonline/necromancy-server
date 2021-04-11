using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyLocalItemobjectData : PacketResponse
    {
        /// <summary>
        /// Spawns some basic world objects. like boxes and barrels.   1 to 9 are acceptable serial IDs
        /// </summary>
        private Character _character;
        private int _serialId;
        public RecvDataNotifyLocalItemobjectData(Character character, int serialId)
            : base((ushort) AreaPacketId.recv_data_notify_local_itemobject_data, ServerType.Area)
        {
            _character = character;
            _serialId = serialId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_character.instanceId); // Object ID

            res.WriteFloat(_character.x);// x
            res.WriteFloat(_character.y);// y
            res.WriteFloat(_character.z +100);// z
            res.WriteByte(_character.heading); //heading

            res.WriteInt32(_serialId); //serial ID.  1 is a box

            res.WriteInt32(0b11111111); //state
            return res;
        }
    }
}
