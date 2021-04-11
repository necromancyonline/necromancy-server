using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_change_role : ClientHandler
    {
        public send_union_request_change_role(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_request_change_role;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint previousLeaderCharacterInstanceId =
                packet.Data.ReadUInt32(); //Old Leader Instance ID if changing leader. otherwise 0
            uint targetInstanceId = packet.Data.ReadUInt32();
            uint targetRole = packet.Data.ReadUInt32(); //3 beginer, 2 member, 1 sub-leader, 0 leader

            // TODO why not retrieve via GetInstance??
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(targetInstanceId);
            Character targetCharacter = targetClient.Character;
            if (targetCharacter == null)
            {
                return;
            }
            
            UnionMember unionMember = Server.Database.SelectUnionMemberByCharacterId(targetCharacter.Id);
            uint previousRank = unionMember.Rank;
            unionMember.Rank = targetRole;
            Server.Database.UpdateUnionMember(unionMember);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check
            Router.Send(client, (ushort) MsgPacketId.recv_union_request_change_role_r, res, ServerType.Msg);


            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(targetInstanceId);
            res2.WriteUInt32(previousLeaderCharacterInstanceId);
            res2.WriteUInt32(targetRole);
            if (targetClient != null)
                Router.Send(targetClient, (ushort) MsgPacketId.recv_union_notify_changed_role, res2, ServerType.Msg);

            if (previousLeaderCharacterInstanceId > 0)
            {
                // TODO why not retrieve via GetInstance??
                NecClient oldLeaderClient = Server.Clients.GetByCharacterInstanceId(previousLeaderCharacterInstanceId);
                Character oldLeaderCharacter = targetClient.Character;
                if (oldLeaderCharacter == null)
                {
                    return;
                }
         
                
                
                UnionMember oldLeaderMember = Server.Database.SelectUnionMemberByCharacterId(oldLeaderCharacter.Id);
                oldLeaderMember.Rank = previousRank;
                Server.Database.UpdateUnionMember(oldLeaderMember);
                IBuffer res3 = BufferProvider.Provide();
                res3.WriteUInt32(targetInstanceId);
                res3.WriteUInt32(previousLeaderCharacterInstanceId);
                res3.WriteUInt32(targetRole);
                if (oldLeaderClient != null)
                    Router.Send(oldLeaderClient, (ushort) MsgPacketId.recv_union_notify_changed_role, res3,
                        ServerType.Msg);
            }
        }
    }
}
