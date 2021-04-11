using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendAuctionRegistSearchEquipmentCond : ClientHandler
    {
        public SendAuctionRegistSearchEquipmentCond(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_auction_regist_search_equipment_cond;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte presetNum = packet.data.ReadByte();
            string keyword = packet.data.ReadFixedString(0x49);//Fixed string of 0x49 or 0x49 bytes.
            byte option1 = packet.data.ReadByte();
            byte option2 = packet.data.ReadByte();
            byte option3 = packet.data.ReadByte();
            byte option4 = packet.data.ReadByte();
            int unknown = packet.data.ReadInt32();
            short stat1 = packet.data.ReadInt16(); //Stat of some sorts?
            short stat2 = packet.data.ReadInt16(); //Stat of some sorts?
            long gold = packet.data.ReadInt64(); //Gold??
            byte option5 = packet.data.ReadByte();
            byte checkBox = packet.data.ReadByte(); //Bool

            byte option6 = packet.data.ReadByte();//These are 3 separate bytes or a fixed string of 3 characters.
            byte option7 = packet.data.ReadByte();//
            byte option8 = packet.data.ReadByte();//

            long gold2 = packet.data.ReadInt64(); //Gold??
            long gold3 = packet.data.ReadInt64(); //Gold??
            string presetName = packet.data.ReadFixedString(0xC1);//Fixed string of 0xC1 or 0xC1 bytes.
            byte option9 = packet.data.ReadByte();
            byte option10 = packet.data.ReadByte();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) AreaPacketId.recv_auction_regist_search_equipment_cond_r, res, ServerType.Area);
        }
    }
}
