using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_regist_member_recruit : ClientHandler
    {
        public send_party_regist_member_recruit(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_regist_member_recruit;

       public override void Handle(NecClient client, NecPacket packet)
        {
            int objectiveID = packet.Data.ReadInt32(); // always 0, 1, or 2. based on drop down selection
            int detailsID = packet.Data.ReadInt32(); // Can Be Dungeon ID or Quest ID based on objective
            uint targetID = packet.Data.ReadUInt32(); // Unknown. possbly Target ID???
            byte recruitingNumber = packet.Data.ReadByte(); // 0 unstated, 3 1 person, 4 2 people, 5 3 people
            int recruitingClasses = packet.Data.ReadInt32(); // 0 unstated, +1 Fig, +2 Thf, +4 Mag, +8 Pri (bitmask)
            int other = packet.Data.ReadInt32(); // 0 unstated, 1 Beginners,2, veterans, 3 Casual, 4 leveling, 5 item hunting
            string comment = packet.Data.ReadFixedString(60); //Comment Box accepts up to 60 characters. 
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
                        
            Router.Send(client, (ushort)AreaPacketId.recv_party_regist_member_recruit_r, res, ServerType.Area);
        }
    }
}
