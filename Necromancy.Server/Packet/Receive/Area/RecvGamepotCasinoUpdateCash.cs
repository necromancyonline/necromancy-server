using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGamepotCasinoUpdateCash : PacketResponse
    {
        public RecvGamepotCasinoUpdateCash()
            : base((ushort) AreaPacketId.recv_gamepot_casino_update_cash, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt64(0);
            return res;
        }
    }
}
