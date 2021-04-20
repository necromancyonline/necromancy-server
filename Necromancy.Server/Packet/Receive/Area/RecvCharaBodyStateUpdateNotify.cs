using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyStateUpdateNotify : PacketResponse
    {
        /// <summary>
        ///     Use this to modify charabody state on disconnect / reconnect / and body collection.
        /// </summary>
        private readonly uint _id;

        private readonly int _stateFlag;

        public RecvCharaBodyStateUpdateNotify(uint id, int stateFlag)
            : base((ushort)AreaPacketId.recv_charabody_state_update_notify, ServerType.Area)
        {
            _stateFlag = stateFlag;
            _id = id;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id); //body instance Id
            res.WriteInt32(_stateFlag); //0b0 = disconnected backpack, 0b1 = normal, 0b100 = title:On 0b10000=invisible. nothing above ob1 << 5
            return res;
        }
    }
}
