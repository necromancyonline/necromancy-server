using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //failsafe to end events when frozen
    public class SendEventEnd : ServerChatCommand
    {
        public SendEventEnd(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.User;
        public override string key => "EndEvent";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
        }
    }
}
