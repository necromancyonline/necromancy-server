using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyRegistMemberRecruit : ClientHandler
    {
        public SendPartyRegistMemberRecruit(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_party_regist_member_recruit;

       public override void Handle(NecClient client, NecPacket packet)
        {
            int objectiveId = packet.data.ReadInt32(); // always 0, 1, or 2. based on drop down selection
            int detailsId = packet.data.ReadInt32(); // Can Be Dungeon ID or Quest ID based on objective
            uint targetId = packet.data.ReadUInt32(); // Unknown. possbly Target ID???
            byte recruitingNumber = packet.data.ReadByte(); // 0 unstated, 3 1 person, 4 2 people, 5 3 people
            int recruitingClasses = packet.data.ReadInt32(); // 0 unstated, +1 Fig, +2 Thf, +4 Mag, +8 Pri (bitmask)
            int other = packet.data.ReadInt32(); // 0 unstated, 1 Beginners,2, veterans, 3 Casual, 4 leveling, 5 item hunting
            string comment = packet.data.ReadFixedString(60); //Comment Box accepts up to 60 characters.
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            router.Send(client, (ushort)AreaPacketId.recv_party_regist_member_recruit_r, res, ServerType.Area);
        }
    }
}
