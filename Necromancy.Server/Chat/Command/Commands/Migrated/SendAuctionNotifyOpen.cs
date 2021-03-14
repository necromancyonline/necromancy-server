using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens auction house
    public class SendAuctionNotifyOpen : ServerChatCommand
    {
        public SendAuctionNotifyOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(5); // must be <= 5

            int numEntries = 0x5;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0); // Slots 0 to 2
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0); // Lowest
                res.WriteInt32(0); // Buy Now
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(1); // 1 permit to show item in the search section ??
                res.WriteFixedString("Comment", 385); // Comment in the item information
                res.WriteInt16(0); // Bid
                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteInt32(8); // must be< = 8

            int numEntries2 = 0x8;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteByte(0); // Slots 0 to 3
                // res.WriteInt64(0xAAAAAAAA); // this is shown in the structure but for some reason isnt read

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0); // Lowest bid info
                res.WriteInt32(0); // Buy now bid info
                res.WriteFixedString($"{client.Soul.Name}", 49);
                res.WriteByte(1); // Change nothing ??
                res.WriteFixedString("Zgeg", 385); // Comment in the bid info
                res.WriteInt16(0);
                res.WriteInt32(0);

                res.WriteInt32(0); // Bid Amount (bid info seciton)
                res.WriteInt32(0);
            }

            res.WriteByte(0); // bool
            Router.Send(client, (ushort) AreaPacketId.recv_auction_notify_open, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "auct";
    }
}
