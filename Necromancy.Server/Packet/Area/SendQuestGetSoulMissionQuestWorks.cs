using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendQuestGetSoulMissionQuestWorks : ClientHandler
    {
        public SendQuestGetSoulMissionQuestWorks(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_quest_get_soul_mission_quest_works;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);

            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteByte(0);
            res.WriteFixedString("channel 1", 97);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteFixedString("channel 2", 97);
            res.WriteByte(1); //bool
            res.WriteByte(1); //bool
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);

            //loop x 10

            for (int i = 0; i < 10; i++)
            {
                res.WriteInt32(1);
                res.WriteFixedString("quest1", 16);
                res.WriteInt16(1);
                res.WriteInt32(1);
            }

            res.WriteByte(1);

            //loop x 12

            for (int i = 0; i < 12; i++)
            {
                res.WriteInt32(1);
                res.WriteFixedString("quest2", 16);
                res.WriteInt16(1);
                res.WriteInt32(1);
            }

            res.WriteByte(1);

            res.WriteFixedString("helppls", 385);
            res.WriteInt64(1);
            res.WriteByte(1);
            res.WriteFixedString("helpplsstill", 385);

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
            //Commented out until further testing. this is called at login
            //Router.Send(client, (ushort)AreaPacketId.recv_quest_get_soul_mission_quest_works_r, res, ServerType.Area);

            //SendQuestDisplay(client);
        }

        private void SendQuestDisplay(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0);

            router.Send(client, (ushort)AreaPacketId.recv_quest_display_r, res2, ServerType.Area);
        }
    }
}
