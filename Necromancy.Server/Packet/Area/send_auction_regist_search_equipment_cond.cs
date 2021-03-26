using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_regist_search_equipment_cond : ClientHandler
    {
        public send_auction_regist_search_equipment_cond(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_regist_search_equipment_cond;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte presetNum = packet.Data.ReadByte();
            string keyword = packet.Data.ReadFixedString(0x49);//Fixed string of 0x49 or 0x49 bytes.
            byte option1 = packet.Data.ReadByte();
            byte option2 = packet.Data.ReadByte();
            byte option3 = packet.Data.ReadByte();
            byte option4 = packet.Data.ReadByte();
            int unknown = packet.Data.ReadInt32();
            short stat1 = packet.Data.ReadInt16(); //Stat of some sorts?
            short stat2 = packet.Data.ReadInt16(); //Stat of some sorts?
            long gold = packet.Data.ReadInt64(); //Gold??
            byte option5 = packet.Data.ReadByte();
            byte checkBox = packet.Data.ReadByte(); //Bool

            byte option6 = packet.Data.ReadByte();//These are 3 separate bytes or a fixed string of 3 characters.
            byte option7 = packet.Data.ReadByte();//
            byte option8 = packet.Data.ReadByte();//

            long gold2 = packet.Data.ReadInt64(); //Gold??
            long gold3 = packet.Data.ReadInt64(); //Gold??
            string presetName = packet.Data.ReadFixedString(0xC1);//Fixed string of 0xC1 or 0xC1 bytes.
            byte option9 = packet.Data.ReadByte();
            byte option10 = packet.Data.ReadByte();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_auction_regist_search_equipment_cond_r, res, ServerType.Area);
        }
    }
}
