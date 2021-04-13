using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestChapterTargetUpdate : PacketResponse
    {
        public RecvQuestChapterTargetUpdate()
            : base((ushort)AreaPacketId.recv_quest_chapter_target_updated, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            return res;
        }
    }
}
