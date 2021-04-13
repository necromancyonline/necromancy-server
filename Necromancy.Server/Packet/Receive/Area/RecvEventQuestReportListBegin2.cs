using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventQuestReportListBegin2 : PacketResponse
    {
        public RecvEventQuestReportListBegin2()
            : base((ushort)AreaPacketId.recv_event_quest_report_list_begin2, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0x1E); // cmp to 0x1E = 30

            int numEntries = 0x1E;
            for (int i = 0; i < numEntries; i++) res.WriteInt32(0);

            res.WriteInt32(1); // cmp to 0x1 = 1 

            res.WriteInt32(0);

            res.WriteInt32(0xA); // cmp to 0xA = 10

            int numEntries2 = 0xA;
            for (int i = 0; i < numEntries2; i++) res.WriteInt32(0);
            return res;
        }
    }
}
