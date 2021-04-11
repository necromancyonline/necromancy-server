using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartySearchRecruitedMember : ClientHandler
    {
        public SendPartySearchRecruitedMember(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_party_search_recruited_member;

       public override void Handle(NecClient client, NecPacket packet)
        {
            uint objective = packet.data.ReadUInt32();
            int details = packet.data.ReadInt32();
            int unknown = packet.data.ReadInt32();
            uint otherCheckBoxSelection = packet.data.ReadUInt32();
            //string Comment = packet.Data.ReadFixedString(60);

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            res.WriteInt32(0x14); // cmp to 0x14 = 20


            int numEntries = 0x14;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(i); //Party ID?
                res.WriteUInt32(client.character.instanceId);
                res.WriteFixedString(client.soul.name, 49);
                res.WriteFixedString(client.character.name, 91);
                res.WriteUInt32(client.character.classId); //Class
                res.WriteByte((byte)(client.character.level+i)); //Level
                res.WriteByte(2); //Criminal Status
                res.WriteByte(1); //Beginner Protection (bool)
                res.WriteByte(0); //Membership Status
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("Comment in Search for Parties Dialog", 181);

            }

            router.Send(client, (ushort)AreaPacketId.recv_party_search_recruited_member_r, res, ServerType.Area);
        }
    }
}
