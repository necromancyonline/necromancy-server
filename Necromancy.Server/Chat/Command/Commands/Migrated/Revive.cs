using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class Revive : ServerChatCommand
    {
        public Revive(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res0 = BufferProvider.Provide();
            res0.WriteInt32(0); //1 = cinematic, 0 Just start the event without cinematic
            res0.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_start, res0, ServerType.Area);

            IBuffer res15 = BufferProvider.Provide();
            //recv_raisescale_view_open = 0xC25D, // Parent = 0xC2E5 // Range ID = 01  // was 0xC25D
            res15.WriteInt16(75); //Basic revival rate %
            res15.WriteInt16(0); //Penalty %
            res15.WriteInt16(2); //Offered item % (this probably changes with recv_raisescale_update_success_per)
            res15.WriteInt16(0); //Dimento medal addition %
            router.Send(client, (ushort)AreaPacketId.recv_raisescale_view_open, res15, ServerType.Area);

        }

        public override AccountStateType accountState => AccountStateType.User;
        public override string key => "revi";
    }
}
