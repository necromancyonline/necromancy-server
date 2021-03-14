using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //Opens Jail Warden dialog to pay bail (toilet?)
    public class SendWantedJailOpen : ServerChatCommand
    {
        public SendWantedJailOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); // 1 make all 3 option availabe ?  and 0 unvailable ?

            res.WriteInt64(70); // Bail
            Router.Send(client, (ushort) AreaPacketId.recv_wanted_jail_open, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "jail";
    }
}
