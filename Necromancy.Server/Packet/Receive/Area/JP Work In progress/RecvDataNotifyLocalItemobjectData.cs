using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_data_notify_local_itemobject_data : PacketResponse
    {
        /// <summary>
        /// Spawns some basic world objects. like boxes and barrels.   1 to 9 are acceptable serial IDs
        /// </summary>
        private Character _character;
        private int _serialId;
        public recv_data_notify_local_itemobject_data(Character character, int serialId)
            : base((ushort) AreaPacketId.recv_data_notify_local_itemobject_data, ServerType.Area)
        {
            _character = character;
            _serialId = serialId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_character.InstanceId); // Object ID

            res.WriteFloat(_character.X);// x
            res.WriteFloat(_character.Y);// y
            res.WriteFloat(_character.Z +100);// z
            res.WriteByte(_character.Heading); //heading

            res.WriteInt32(_serialId); //serial ID.  1 is a box

            res.WriteInt32(0b11111111); //state
            return res;
        }
    }
}
