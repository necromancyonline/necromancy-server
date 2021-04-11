using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //Picks up your own body in to soul storage
    public class SendSalvageNotifyBody : ServerChatCommand
    {
        public SendSalvageNotifyBody(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide(); // it appear in the collected body
            res.WriteInt32(1); //  slots
            res.WriteCString($"{client.soul.name}"); // Soul Name
            res.WriteCString($"{client.character.name}"); // Character Name
            router.Send(client, (ushort) AreaPacketId.recv_charabody_salvage_notify_body, res, ServerType.Area);
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "salv";
    }
}
