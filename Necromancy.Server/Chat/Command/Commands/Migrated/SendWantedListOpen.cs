using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendWantedListOpen : ServerChatCommand
    {
        //Opens Bounty Board
        public SendWantedListOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(9999); // Bounty points.
            res.WriteInt32(0);
            res.WriteInt32(0); // When i change list doesn't open anymore, don't know what is it
            Router.Send(client, (ushort) AreaPacketId.recv_wanted_list_open, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "list";
    }
}
