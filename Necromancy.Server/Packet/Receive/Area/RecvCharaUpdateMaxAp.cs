using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateMaxAp : PacketResponse
    {
        private int _ap;
        public RecvCharaUpdateMaxAp(int ap)
            : base((ushort) AreaPacketId.recv_chara_update_maxap, ServerType.Area)
        {
            _ap = ap;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_ap);
            return res;
        }
    }
}
