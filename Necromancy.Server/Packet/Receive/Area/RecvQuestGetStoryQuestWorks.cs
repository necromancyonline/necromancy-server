using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestGetStoryQuestWorks : PacketResponse
    {
        public RecvQuestGetStoryQuestWorks()
            : base((ushort)AreaPacketId.recv_quest_get_story_quest_works_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);

            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteByte(0);
            res.WriteFixedString("channel 1", 97);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteFixedString("channel 2", 97);
            res.WriteByte(1); //bool
            res.WriteByte(1); //bool
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);

            //loop x 10
            for (int i = 0; i < 10; i++)
            {
                res.WriteInt32(1);
                res.WriteFixedString("quest1", 16);
                res.WriteInt16(1);
                res.WriteInt32(1);
            }

            res.WriteByte(1);

            //loop x 12
            for (int i = 0; i < 12; i++)
            {
                res.WriteInt32(1);
                res.WriteFixedString("quest2", 16);
                res.WriteInt16(1);
                res.WriteInt32(1);
            }

            res.WriteByte(1);
            res.WriteFixedString("idk", 385);
            res.WriteInt64(1);
            res.WriteByte(1);
            res.WriteFixedString("idk_also", 385);

            for (int i = 0; i < 5; i++)
            {
                res.WriteByte(1);
                res.WriteInt32(1);
                res.WriteInt32(1);
                res.WriteInt32(1);
                res.WriteInt32(1);
                res.WriteInt32(1);
            }

            res.WriteByte(1);
            res.WriteInt32(1);
            res.WriteFloat(1);
            return res;
        }
    }
}
