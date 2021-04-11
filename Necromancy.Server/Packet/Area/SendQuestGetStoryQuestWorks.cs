using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_quest_get_story_quest_works : ClientHandler
    {
        public send_quest_get_story_quest_works(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_quest_get_story_quest_works;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(10);

            res.WriteInt32(1);//less than or equal to 0x1


            res.WriteInt32(1);//Needs to be 1 or you get a DC
            res.WriteByte(0);
            res.WriteByte(0);//new
            res.WriteFixedString("Quest of dying", 97);//Quest name
            res.WriteInt32(25); //Level requirement
            res.WriteInt32(1);
            res.WriteFixedString("Death", 97);//Client name
            res.WriteByte(0); //bool
            res.WriteByte(0); //Bool for if the mission can be abandoned or not.
            res.WriteInt32(1);
            res.WriteInt32(50); //EXP
            res.WriteInt32(75); //Gold Pieces
            res.WriteInt32(100); //Skill Points
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new
            res.WriteInt32(0);//new

            //loop x 10
            //Some  sort of ITEM info
            uint bitShift = 4194304;
            uint odd = 1;
            for (int i = 0; i < 10; i++)
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
            res.WriteByte(10); //# for reward

            //loop x 12
            //Some  sort of ITEM info
            for (int i = 0; i < 12; i++)
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

            for (int i = 0; i < 5; i++)
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

            Router.Send(client, (ushort) AreaPacketId.recv_quest_get_story_quest_works_r, res, ServerType.Area);

            //SendQuestDisplay(client);
        }

        private void SendQuestDisplay(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_quest_display_r, res2, ServerType.Area);


        }
    }
}
