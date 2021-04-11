using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartySearchRecruitedParty : ClientHandler
    {
        public SendPartySearchRecruitedParty(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_party_search_recruited_party;

       public override void Handle(NecClient client, NecPacket packet)
        {
            uint objective = packet.data.ReadUInt32();
            int details = packet.data.ReadInt32();
            int unknown = packet.data.ReadInt32();
            uint otherCheckBoxSelection = packet.data.ReadUInt32();
            string comment = packet.data.ReadFixedString(60);


            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.character.instanceId);
            res.WriteInt32(0x14); // cmp to 0x14

            int numEntries = 0x14;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(510+i);//Party ID
                res.WriteInt32(1);//Party type; 0 = closed, 1 = open.
                res.WriteInt32(1);//Normal item distribution; 0 = do not distribute, 1 = random.
                res.WriteInt32(1);//Rare item distribution; 0 = do not distribute, 1 = Draw.
                res.WriteUInt32(client.character.instanceId); //Target playyer?
                res.WriteUInt32(client.character.instanceId);//From player instance ID (but doesn't work?)

                int numEntries2 = 0x4;
                for (int j = 0; j < numEntries2; j++)
                {
                    res.WriteInt32(0); //?
                    res.WriteUInt32(client.character.instanceId);
                    res.WriteFixedString($"{client.soul.name}{j}", 0x31);
                    res.WriteFixedString($"{client.character.name}{j}", 0x5B);
                    res.WriteUInt32(client.character.classId);
                    res.WriteByte(client.character.level);
                    res.WriteByte(2); //Criminal Status
                    res.WriteByte(1); //Beginner Protection (bool)
                    res.WriteByte(1); //Membership Status
                    res.WriteByte(1);
                }

                res.WriteByte(3); //party member count per listed party

                res.WriteUInt32(objective); //objective per listed party
                res.WriteInt32(details);
                res.WriteInt32(unknown);
                res.WriteByte(0); //party match requirements. 1 for no match error
                res.WriteInt32(i); //recruiting classes per listed party

                res.WriteInt32(i); //"other" column or objective per listed party

                res.WriteFixedString($"Party Comment Box : Loop{i}", 0xB5);
            }

            router.Send(client, (ushort)AreaPacketId.recv_party_search_recruited_party_r, res, ServerType.Area);
        }
    }
}
