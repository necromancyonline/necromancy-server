using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    ///     Character Arrange stuff.
    /// </summary>
    public class ArrangeCommand : ServerChatCommand
    {
        public ArrangeCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "arrange";
        public override string helpText => "usage: `/arrange parts` - whatever chara arrange does.";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == "")
            {
                responses.Add(ChatResponse.CommandError(client, $"pick a command: {command[0]}"));
                return;
            }

            IBuffer res = BufferProvider.Provide();

            switch (command[0])
            {
                case "open":
                    RecvCharaArrangeNotifyOpen openArrange = new RecvCharaArrangeNotifyOpen();
                    RecvCharaArrangeNotifyUpdateUnlock unlockArrange1 = new RecvCharaArrangeNotifyUpdateUnlock();
                    router.Send(unlockArrange1, client);
                    router.Send(openArrange, client);

                    break;

                case "update":
                    RecvCharaArrangeUpdateFormR updateArrange = new RecvCharaArrangeUpdateFormR();
                    router.Send(updateArrange, client);
                    break;

                case "parts":
                    RecvCharaArrangeNotifyParts partsArrange = new RecvCharaArrangeNotifyParts();
                    router.Send(partsArrange, client);
                    break;

                case "unlock":
                    RecvCharaArrangeNotifyUpdateUnlock unlockArrange = new RecvCharaArrangeNotifyUpdateUnlock();
                    router.Send(unlockArrange, client);
                    break;

                case "form":
                    RecvCharaArrangeUpdateFormR formArrange = new RecvCharaArrangeUpdateFormR();
                    router.Send(formArrange, client);
                    break;

                default:
                    Task.Delay(TimeSpan.FromMilliseconds(10 * 1000)).ContinueWith
                    (t1 =>
                        {
                            IBuffer res = BufferProvider.Provide();
                            res.WriteByte(0);
                            router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
                        }
                    );
                    break;
            }
        }
    }

    //res.WriteInt32(numEntries); //less than 0x1E
    //res.WriteInt32(0);
    //res.WriteInt64(0);
    //res.WriteInt16(0);
    //res.WriteByte(0);
    //res.WriteFixedString("Xeno", 0x10);
    //res.WriteCString("What");
    //res.WriteFloat(0);
}
