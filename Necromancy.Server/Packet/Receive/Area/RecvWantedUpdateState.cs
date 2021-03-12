using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvWantedUpdateState : PacketResponse
    {
        private int _wantedState;
        public RecvWantedUpdateState(int wantedState)
            : base((ushort) AreaPacketId.recv_wanted_update_state, ServerType.Area)
        {
            _wantedState = wantedState;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_wantedState);
            return res;
        }
    }
}
