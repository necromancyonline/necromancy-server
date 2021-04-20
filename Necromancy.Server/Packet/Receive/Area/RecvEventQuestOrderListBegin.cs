using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventQuestOrderListBegin : PacketResponse
    {
        public RecvEventQuestOrderListBegin()
            : base((ushort)AreaPacketId.recv_event_quest_order_list_begin, ServerType.Area)
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
                res.WriteFixedString("", 0x61);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x61);
                res.WriteByte(0); //bool
                res.WriteByte(0); //bool
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
                }

                res.WriteByte(0);
                for (int k = 0; k < 0xC; k++)
                {
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x10);
                    res.WriteInt16(0);
                    res.WriteInt32(0);
                }

                res.WriteByte(0);

                res.WriteFixedString("", 0x181);
                res.WriteFixedString("", 0x181);
                for (int l = 0; l < 0x5; l++)
                {
                    res.WriteByte(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                }

                res.WriteByte(0);
            }

            res.WriteInt32(0);
            return res;
        }
    }
}
