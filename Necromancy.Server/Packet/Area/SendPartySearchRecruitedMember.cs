using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_search_recruited_member : ClientHandler
    {
        public send_party_search_recruited_member(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_search_recruited_member;

       public override void Handle(NecClient client, NecPacket packet)
        {
            uint Objective = packet.Data.ReadUInt32();
            int Details = packet.Data.ReadInt32();
            int Unknown = packet.Data.ReadInt32();
            uint OtherCheckBoxSelection = packet.Data.ReadUInt32();
            //string Comment = packet.Data.ReadFixedString(60);

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            res.WriteInt32(0x14); // cmp to 0x14 = 20


            int numEntries = 0x14;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(i); //Party ID?
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteFixedString(client.Soul.Name, 49);
                res.WriteFixedString(client.Character.Name, 91);
                res.WriteUInt32(client.Character.ClassId); //Class
                res.WriteByte((byte)(client.Character.Level+i)); //Level
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

            Router.Send(client, (ushort)AreaPacketId.recv_party_search_recruited_member_r, res, ServerType.Area);
        }
    }
}
