using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens an empty Treasure box window
    public class SendEventTreasureboxBegin : ServerChatCommand
    {
        public SendEventTreasureboxBegin(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0); // 1 = cinematic
            res2.WriteByte(0);

            Router.Send(client, (ushort) AreaPacketId.recv_event_start, res2, ServerType.Area);

            //recv_event_tresurebox_begin = 0xBD7E,
            IBuffer res1 = BufferProvider.Provide();
            int numEntries = 0x10;
            res1.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++)
            {
                res1.WriteInt32(10001 + i);
            }

            Router.Send(client, (ushort) AreaPacketId.recv_event_treasurebox_begin, res1, ServerType.Area);


            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(0); // 1 = Error reported by SV,  1 = sucess
            Router.Send(client, (ushort) AreaPacketId.recv_event_treasurebox_select_r, res4, ServerType.Area);
            
            /*   IBuffer res4 = BufferProvider.Provide();
               res4.WriteByte(3);
               Router.Send(client, (ushort)AreaPacketId.recv_event_end, res4); */
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "tbox";
    }
}
