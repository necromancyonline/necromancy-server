using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendQuestGetMissionQuestWorks : ClientHandler
    {
        public SendQuestGetMissionQuestWorks(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_quest_get_mission_quest_works;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            int numEntries = 0x1; //0x1E
            res.WriteInt32(numEntries);//less than or equal to 0x1E   Character.MissionQuests.Count;

            for(int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(1);//Needs to be 1 or you get a DC
                res.WriteByte(0);
                res.WriteByte(0);//new
                res.WriteFixedString("Quest of dying", 97);//Quest name
                res.WriteInt32(25); //Level requirement
                res.WriteInt32(1);
                res.WriteFixedString("Death", 97);//Client name
                res.WriteByte(0); //bool
                res.WriteByte(1); //Bool for if the mission can be abandoned or not.
                res.WriteInt32(1);
                res.WriteInt32(50); //EXP
                res.WriteInt32(75); //Gold Pieces
                res.WriteInt32(100); //Skill Points
                res.WriteInt32(1);//new
                res.WriteInt32(2);//new
                res.WriteInt32(3);//new
                res.WriteInt32(4);//new

                //loop x 10
                //Some  sort of ITEM info
                uint bitShift = 4194304;
                uint odd = 1;
                for (int j = 0; j < 10; j++)
                {
                    res.WriteInt32(10200101); //Item icon ID
                    res.WriteFixedString("quest1", 16); //Item name?
                    res.WriteInt16(1); //Quantity
                    res.WriteUInt32(odd); //Status
                    res.WriteByte(1);//new
                    res.WriteInt16(11);//new
                    bitShift = bitShift << 1;
                    odd += 2;
                }
                res.WriteByte(10); //# for reward

                //loop x 12
                //Some  sort of ITEM info
                for (int j = 0; j < 12; j++)
                {
                    res.WriteInt32(10200101); //Item icon ID
                    res.WriteFixedString("quest1", 16); //Item name?
                    res.WriteInt16(0); //Quantity
                    res.WriteUInt32(odd); //Status
                    res.WriteByte(0);//new
                    res.WriteInt16(0);//new
                    bitShift = bitShift << 1;
                    odd += 2;
                }
                res.WriteByte(12);//# of "selected prize"
                res.WriteInt32(0);//new


                res.WriteFixedString("Get some mobs to kill you.", 385); //Quest description
                res.WriteInt64(2036854775807); //Time left #; maybe unix? Default on screenshots seem to be 6h
                res.WriteByte(0);
                res.WriteFixedString("Go to map 1001902 and have the mobs kill you.", 385); //Brief quest objective

                for (int j = 0; j < 5; j++)
                {
                    res.WriteByte(1);
                    res.WriteInt32(1);
                    res.WriteInt32(1);
                    res.WriteInt32(1);

                    res.WriteInt32(1);

                    res.WriteInt32(1);

                }

                res.WriteByte(1);

                res.WriteInt32(1);

                res.WriteFloat(1);

                res.WriteByte(0);//new
                res.WriteByte(0);//new
                res.WriteInt16(0);//new
                res.WriteInt16(0);//new
                res.WriteInt32(0);//new
            }

            //Commented out until further testing. this is called at login
            router.Send(client, (ushort) AreaPacketId.recv_quest_get_mission_quest_works_r, res, ServerType.Area);
        }
    }
}
