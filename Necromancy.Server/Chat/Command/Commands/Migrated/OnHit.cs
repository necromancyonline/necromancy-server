using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //  battle report attack on hit.
    public class OnHit : ServerChatCommand
    {
        public OnHit(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "OnHit";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_attack_onhit, res,
                ServerType.Area);
        }
    }
}
