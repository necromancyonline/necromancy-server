using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Character Arrange stuff.
    /// </summary>
    public class ArrangeCommand : ServerChatCommand
    {
        public ArrangeCommand(NecServer server) : base(server)
        {
        }

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
                    recv_chara_arrange_notify_open openArrange = new recv_chara_arrange_notify_open();
                    recv_chara_arrange_notify_update_unlock unlockArrange1 = new recv_chara_arrange_notify_update_unlock();
                    Router.Send(unlockArrange1, client);
                    Router.Send(openArrange, client);

                    break;

                case "update":
                    recv_chara_arrange_update_form_r updateArrange = new recv_chara_arrange_update_form_r();
                    Router.Send(updateArrange, client);
                    break;

                case "parts":
                    recv_chara_arrange_notify_parts partsArrange = new recv_chara_arrange_notify_parts();
                    Router.Send(partsArrange, client);
                    break;

                case "unlock":
                    recv_chara_arrange_notify_update_unlock unlockArrange = new recv_chara_arrange_notify_update_unlock();
                    Router.Send(unlockArrange, client);
                    break;

                case "form":
                    recv_chara_arrange_update_form_r formArrange = new recv_chara_arrange_update_form_r();
                    Router.Send(formArrange, client);
                    break;

                default:
                    Task.Delay(TimeSpan.FromMilliseconds((int)(10 * 1000))).ContinueWith
                    (t1 =>
                    {
                        IBuffer res = BufferProvider.Provide();
                        res.WriteByte(0);
                        Router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
                    }
                    );
                    break;
            }

        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "arrange";
        public override string HelpText => "usage: `/arrange parts` - whatever chara arrange does.";
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
