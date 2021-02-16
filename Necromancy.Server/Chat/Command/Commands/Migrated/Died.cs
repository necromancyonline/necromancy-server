using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    //displays message that you died
    public class Died : ServerChatCommand
    {
        public Died(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (client.Character.HasDied == true)
            {
                IBuffer res4 = BufferProvider.Provide();
                Router.Send(client.Map, (ushort) AreaPacketId.recv_self_lost_notify, res4, ServerType.Area);
            }

            if (client.Character.HasDied == false)
            {
                client.Character.HasDied = true; // setting before the Sleep so other monsters can't "kill you" while you're dieing
                client.Character.Hp.Modify(-client.Character.Hp.current);


            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "Died";
    }
}
