using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvRefusalListNotifyRemoveUserSoulDelete : PacketResponse
    {
        public RecvRefusalListNotifyRemoveUserSoulDelete()
            : base((ushort)MsgPacketId.recv_refusallist_notify_remove_user_souldelete, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            return res;
        }
    }
}
