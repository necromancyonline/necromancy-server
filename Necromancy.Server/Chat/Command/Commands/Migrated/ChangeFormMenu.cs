using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class ChangeFormMenu : ServerChatCommand
    {
        public ChangeFormMenu(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "form";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (client.character.eventSelectExecCode == -1)
            {
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt32(0); //1 = cinematic, 0 Just start the event without cinematic
                res2.WriteByte(0);


                router.Send(client, (ushort)AreaPacketId.recv_event_start, res2, ServerType.Area);

                IBuffer res3 = BufferProvider.Provide();
                res3.WriteCString("Male"); //Length 0x601
                router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res3,
                    ServerType.Area); // It's the first choice

                IBuffer res70 = BufferProvider.Provide();
                res70.WriteCString("Female"); //Length 0x601
                router.Send(client, (ushort)AreaPacketId.recv_event_select_push, res70,
                    ServerType.Area); // It's the second choice


                IBuffer res1 = BufferProvider.Provide();

                res1.WriteCString("Character appareance change \n Price : 9000 Pieces"); // It's the title dude
                res1.WriteInt32(6); // This is the Event Type.  0xFFFD sends a 58 byte packet
                router.Send(client, (ushort)AreaPacketId.recv_event_select_exec, res1, ServerType.Area);
            }

            if (client.character.eventSelectExecCode != -1)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteUInt32(client.character.raceId); // race
                res.WriteInt32(client.character.eventSelectExecCode); // gender 0 = female, 1 = male
                res.WriteByte(client.character.hairId); //hair
                res.WriteByte(client.character.hairColorId); //color
                res.WriteByte(client.character.faceId); //face
                router.Send(client, (ushort)AreaPacketId.recv_chara_update_form, res, ServerType.Area);
                SendEventEnd(client);
                client.character.eventSelectExecCode = -1;
            }
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
        }
    }
}
