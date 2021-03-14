using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SoulShop : ServerChatCommand
    {
        public SoulShop(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_soulmaterial_shop_notify_open, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "soul";
    }
}
