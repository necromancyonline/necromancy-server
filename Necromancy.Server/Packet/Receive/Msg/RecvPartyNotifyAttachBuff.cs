using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyAttachBuff : PacketResponse
    {
        public RecvPartyNotifyAttachBuff()
            : base((ushort)MsgPacketId.recv_party_notify_attach_buff, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(20000002);
            res.WriteInt32(564); //object id?
            res.WriteInt32(79901); //serial id? 
            res.WriteInt32(100); // time?
            return res;
        }
    }
}
