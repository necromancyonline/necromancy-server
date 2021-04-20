using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCashGetUrl : PacketResponse
    {
        public RecvCashGetUrl()
            : base((ushort)MsgPacketId.recv_cash_get_url_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString("");
            return res;
        }
    }
}
