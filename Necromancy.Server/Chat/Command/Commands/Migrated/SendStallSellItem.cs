using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //sells an item from your stall
    public class SendStallSellItem : ServerChatCommand
    {
        public SendStallSellItem(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "sssi";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //recv_stall_sell_item = 0x919C,
            IBuffer res = BufferProvider.Provide();

            res.WriteCString("C1Str"); // find max size Character name/Soul name
            res.WriteCString("CStr2"); // find max size Character name/Soul name
            res.WriteInt64(25);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(16);
            res.WriteUInt32(client.character.instanceId); //Item id

            router.Send(client, (ushort)AreaPacketId.recv_stall_sell_item, res, ServerType.Area);
        }
    }
}
