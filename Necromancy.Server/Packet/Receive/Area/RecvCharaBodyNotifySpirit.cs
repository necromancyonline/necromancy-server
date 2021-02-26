using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifySpirit : PacketResponse
    {
        /// <summary>
        /// Call this recv on disconnect and reconnect of a dead character.  send it to all clients on the map
        /// on disconnect of the client, send byte 0.  on re-connect of the client send byte 1
        /// this is to notify the charabody, so use the deadBody.InstanceId
        /// </summary>
        public enum ValidSpirit : byte
        {
            DisconnectedClient = 0,
            ConnectedClient = 1
        }
        private uint _id;
        private byte _valid;
        public RecvCharaBodyNotifySpirit(uint id, byte valid)
            : base((ushort) AreaPacketId.recv_charabody_notify_spirit, ServerType.Area)
        {
            _id = id;
            _valid = valid;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id); //Dead Body (charabody)  instance ID
            res.WriteByte(_valid); //valid spirit :  0 = Disconnected client . e.g. invalid spirit.  Rucksack model.   \  1 = Connected client. Valid spirit. character model.
            return res;
        }
    }
}
