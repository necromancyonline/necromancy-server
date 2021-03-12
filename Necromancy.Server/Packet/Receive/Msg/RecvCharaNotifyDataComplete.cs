using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCharaNotifyDataComplete : PacketResponse
    {
        public RecvCharaNotifyDataComplete()
            : base((ushort) MsgPacketId.recv_chara_notify_data_complete, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0); //bool
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
