using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyDeadState : PacketResponse
    {
        private uint _id;
        private int _deadState;
        private int _changeType;
        public RecvCharaBodyNotifyDeadState(uint id, int deadState, int changeType)
            : base((ushort) AreaPacketId.recv_charabody_notify_deadstate, ServerType.Area)
        {
            _id = id;
            _deadState = deadState;
            _changeType = changeType;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id); //CharaBody Instance Id
            res.WriteInt32(_deadState); //deadstate      : //4 changes body to ash pile, 5 causes a mist to happen and disappear (Lost)
            res.WriteInt32(_changeType); //change_type
            return res;
        }
    }
}
