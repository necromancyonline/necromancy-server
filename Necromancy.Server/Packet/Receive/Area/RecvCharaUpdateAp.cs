using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateAp : PacketResponse
    {
        private int _ap;
        public RecvCharaUpdateAp(int ap)
            : base((ushort) AreaPacketId.recv_chara_update_ap, ServerType.Area)
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
