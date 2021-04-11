using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_quest_report_select : ClientHandler
    {
        public send_event_quest_report_select(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_quest_report_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int questId = packet.Data.ReadInt32(); 
            int prizeSlot = packet.Data.ReadInt32(); 



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
