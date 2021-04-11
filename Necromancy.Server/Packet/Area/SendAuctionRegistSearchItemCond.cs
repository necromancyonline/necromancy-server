using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendAuctionRegistSearchItemCond : ClientHandler
    {
        public SendAuctionRegistSearchItemCond(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_auction_regist_search_item_cond;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte presetNum = packet.data.ReadByte();
            string keyword = packet.data.ReadFixedString(0x49);//Fixed string of 0x49 or 0x49 bytes.
            byte option1 = packet.data.ReadByte();
            byte option2 = packet.data.ReadByte();
            byte option3 = packet.data.ReadByte();
            byte option4 = packet.data.ReadByte();
            long gold = packet.data.ReadInt64(); //Gold??
            byte option5 = packet.data.ReadByte();
            long gold2 = packet.data.ReadInt64(); //Gold??
            long gold3 = packet.data.ReadInt64(); //Gold??
            string presetName = packet.data.ReadFixedString(0xC1);//Fixed string of 0xC1 or 0xC1 bytes.
            byte option6 = packet.data.ReadByte();
            byte option7 = packet.data.ReadByte();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) AreaPacketId.recv_auction_regist_search_item_cond_r, res, ServerType.Area);
        }
    }
}
