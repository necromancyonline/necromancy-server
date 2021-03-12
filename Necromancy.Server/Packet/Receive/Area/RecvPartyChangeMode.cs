using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyChangeMode : PacketResponse
    {
        int _mode;
        public RecvPartyChangeMode(int mode)
            : base((ushort) AreaPacketId.recv_party_change_mode_r, ServerType.Area)
        {
            _mode = mode;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_mode);
            return res;
        }
    }
}
