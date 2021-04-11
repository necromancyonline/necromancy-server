using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventQuestReportSelect : ClientHandler
    {
        public SendEventQuestReportSelect(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_event_quest_report_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int questId = packet.data.ReadInt32();
            int prizeSlot = packet.data.ReadInt32();



            IBuffer res = BufferProvider.Provide();

            //remove quest
            //receive item
            //add completed quest to history
            //increase exp
            //increase soul points
            //next quest hint
            //etc.....


        }

    }
}
