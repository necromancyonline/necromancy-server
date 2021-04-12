using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateMentorLv : PacketResponse
    {
        public RecvCharaUpdateMentorLv()
            : base((ushort)AreaPacketId.recv_chara_update_mentor_lv, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);

            return res;
        }
    }
}
