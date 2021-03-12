using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvWantedUpdateStateNotify : PacketResponse
    {
        private int _instanceId; //???
        private int _wantedState; //???
        public RecvWantedUpdateStateNotify(int instanceId, int wantedState)
            : base((ushort) AreaPacketId.recv_wanted_update_state_notify, ServerType.Area)
        {
            _instanceId = instanceId;
            _wantedState = wantedState;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_instanceId);
            res.WriteInt32(_wantedState);
            return res;
        }
    }
}
