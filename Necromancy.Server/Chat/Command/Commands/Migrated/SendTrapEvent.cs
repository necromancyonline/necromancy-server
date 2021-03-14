using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //disarm a red chest trap window
    public class SendTrapEvent : ServerChatCommand
    {
        public SendTrapEvent(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //1 = cinematic, 0 Just start the event without cinematic
            res.WriteByte(0);

            Router.Send(client, (ushort) AreaPacketId.recv_event_start, res, ServerType.Area);

            IBuffer res0 = BufferProvider.Provide();
            res0.WriteCString("Boobs Trap !"); // find max size  Text display at the top of the screen
            res0.WriteInt32(1);
            Router.Send(client, (ushort) AreaPacketId.recv_event_show_board_start, res0, ServerType.Area);


            IBuffer res2 = BufferProvider.Provide();

            res2.WriteInt32(Util.GetRandomNumber(0, 100)); // Percent

            res2.WriteInt32(0);

            res2.WriteByte(1); // bool  change chest image  1 = gold
            Router.Send(client, (ushort) AreaPacketId.recv_event_removetrap_begin, res2, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "trap";
    }
}
