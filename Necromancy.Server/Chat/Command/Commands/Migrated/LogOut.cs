using System.Collections.Generic;
using System.Threading;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    //logs you out
    public class LogOut : ServerChatCommand
    {
        public LogOut(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "logout";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            byte[] byteArr = new byte[8] {0x00, 0x06, 0xEE, 0x91, 0, 0, 0, 0};

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            res.SetPositionStart();

            for (int i = 4; i < 8; i++) byteArr[i] += res.ReadByte();

            // TODO use packet format
            //  client.MsgConnection.Send(byteArr);

            Thread.Sleep(4000);

            byte[] byteArrr = new byte[9] {0x00, 0x07, 0x52, 0x56, 0, 0, 0, 0, 0};

            IBuffer res2 = BufferProvider.Provide();

            res2.WriteInt32(0);
            res2.WriteByte(0);

            res2.SetPositionStart();

            for (int i = 4; i < 9; i++) byteArrr[i] += res2.ReadByte();

            // TODO use packet format
            //  client.MsgConnection.Send(byteArr);
        }
    }
}
