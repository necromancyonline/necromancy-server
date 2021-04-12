using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestChapterUpdated : PacketResponse
    {
        public RecvQuestChapterUpdated()
            : base((ushort)AreaPacketId.recv_quest_chapter_updated, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(""); // Length 0x181


            int numEntries = 0x5;
            res.WriteInt32(numEntries); //less than or equal to 0x5
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteInt32(0);
            res.WriteFloat(0);
            return res;
        }
    }
}
