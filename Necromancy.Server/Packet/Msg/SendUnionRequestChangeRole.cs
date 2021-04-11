using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendUnionRequestChangeRole : ClientHandler
    {
        public SendUnionRequestChangeRole(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_union_request_change_role;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint previousLeaderCharacterInstanceId =
                packet.data.ReadUInt32(); //Old Leader Instance ID if changing leader. otherwise 0
            uint targetInstanceId = packet.data.ReadUInt32();
            uint targetRole = packet.data.ReadUInt32(); //3 beginer, 2 member, 1 sub-leader, 0 leader

            // TODO why not retrieve via GetInstance??
            NecClient targetClient = server.clients.GetByCharacterInstanceId(targetInstanceId);
            Character targetCharacter = targetClient.character;
            if (targetCharacter == null)
            {
                return;
            }

            UnionMember unionMember = server.database.SelectUnionMemberByCharacterId(targetCharacter.id);
            uint previousRank = unionMember.rank;
            unionMember.rank = targetRole;
            server.database.UpdateUnionMember(unionMember);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check
            router.Send(client, (ushort) MsgPacketId.recv_union_request_change_role_r, res, ServerType.Msg);


            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(targetInstanceId);
            res2.WriteUInt32(previousLeaderCharacterInstanceId);
            res2.WriteUInt32(targetRole);
            if (targetClient != null)
                router.Send(targetClient, (ushort) MsgPacketId.recv_union_notify_changed_role, res2, ServerType.Msg);

            if (previousLeaderCharacterInstanceId > 0)
            {
                // TODO why not retrieve via GetInstance??
                NecClient oldLeaderClient = server.clients.GetByCharacterInstanceId(previousLeaderCharacterInstanceId);
                Character oldLeaderCharacter = targetClient.character;
                if (oldLeaderCharacter == null)
                {
                    return;
                }



                UnionMember oldLeaderMember = server.database.SelectUnionMemberByCharacterId(oldLeaderCharacter.id);
                oldLeaderMember.rank = previousRank;
                server.database.UpdateUnionMember(oldLeaderMember);
                IBuffer res3 = BufferProvider.Provide();
                res3.WriteUInt32(targetInstanceId);
                res3.WriteUInt32(previousLeaderCharacterInstanceId);
                res3.WriteUInt32(targetRole);
                if (oldLeaderClient != null)
                    router.Send(oldLeaderClient, (ushort) MsgPacketId.recv_union_notify_changed_role, res3,
                        ServerType.Msg);
            }
        }
    }
}
