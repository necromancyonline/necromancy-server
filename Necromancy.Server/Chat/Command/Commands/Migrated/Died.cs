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
            if (client.character.hasDied == true)
            {
                IBuffer res1 = BufferProvider.Provide();
                res1.WriteUInt32(client.character.instanceId); // ID
                res1.WriteInt64((long)CharacterState.LostState); //
                router.Send(client.map, (ushort)AreaPacketId.recv_chara_notify_stateflag, res1, ServerType.Area);

                client.character.hp.SetCurrent(-2); //This will make you show lost on chara select.

                IBuffer res4 = BufferProvider.Provide();
                router.Send(client, (ushort) AreaPacketId.recv_self_lost_notify, res4, ServerType.Area);
            }

            if (client.character.hasDied == false)
            {
                client.character.hasDied = true;
                client.character.hp.Modify(-client.character.hp.current);


            }
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "Died";
    }
}
