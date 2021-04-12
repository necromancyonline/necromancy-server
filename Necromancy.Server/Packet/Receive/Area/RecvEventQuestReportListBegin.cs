using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventQuestReportListBegin : PacketResponse
    {
        public RecvEventQuestReportListBegin()
            : base((ushort)AreaPacketId.recv_event_quest_report_list_begin, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();

            return res;
        }
    }
}
