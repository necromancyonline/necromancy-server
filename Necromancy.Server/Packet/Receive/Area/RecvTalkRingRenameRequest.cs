using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTalkRingRenameRequest : PacketResponse
    {
        public RecvTalkRingRenameRequest()
            : base((ushort)AreaPacketId.recv_talkring_rename_request, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteCString("ToBeFound");
            return res;
        }
    }
}
