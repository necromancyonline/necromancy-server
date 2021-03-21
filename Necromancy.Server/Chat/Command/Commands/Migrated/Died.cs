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
                IBuffer res1 = BufferProvider.Provide();
                res1.WriteUInt32(client.Character.InstanceId); // ID
                res1.WriteInt64((long)CharacterState.LostState); //
                Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_stateflag, res1, ServerType.Area);

                client.Character.Hp.setCurrent(-2); //This will make you show lost on chara select.

                IBuffer res4 = BufferProvider.Provide();
                Router.Send(client, (ushort) AreaPacketId.recv_self_lost_notify, res4, ServerType.Area);
            }

            if (client.Character.HasDied == false)
            {
                client.Character.HasDied = true;
                client.Character.Hp.Modify(-client.Character.Hp.current);


            }
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "Died";
    }
}
