using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestHistory : PacketResponse
    {
        public RecvQuestHistory()
            : base((ushort) AreaPacketId.recv_quest_history, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x1E;
            res.WriteInt32(numEntries); //less than or equal to 0x1E
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);//New
                res.WriteFixedString("", 0x61);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x61);
                res.WriteByte(0);//bool
                res.WriteByte(0);//bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0xA; j++)
                {
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteInt16(0);
                }
                res.WriteByte(0);
                for (int k = 0; k < 0xC; k++)
                {
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteInt16(0);
                }
                res.WriteByte(0);
                res.WriteFixedString("", 0x181);
                res.WriteInt32(0);//new
            }
            res.WriteInt32(0);
            return res;
        }
    }
}
