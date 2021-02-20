using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBoardNotifyDeadState : PacketResponse
    {
        public RecvCharaBoardNotifyDeadState()
            : base((ushort) AreaPacketId.recv_charabody_notify_deadstate, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //CharaBody Instance Id
            res.WriteInt32(0); //deadstate      : //4 changes body to ash pile, 5 causes a mist to happen and disappear (Lost)
            res.WriteInt32(0); //change_type
            return res;
        }
    }
}
