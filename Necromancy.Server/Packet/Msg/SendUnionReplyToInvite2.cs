using System;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendUnionReplyToInvite2 : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendUnionReplyToInvite2));

        public SendUnionReplyToInvite2(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_union_reply_to_invite2;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint replyToInstanceId = packet.data.ReadUInt32();
            uint resultAcceptOrDeny = packet.data.ReadUInt32();
            NecClient replyToClient = server.clients.GetByCharacterInstanceId(replyToInstanceId);
            _Logger.Debug(
                $"replyToInstanceId {replyToInstanceId} resultAcceptOrDeny {resultAcceptOrDeny} replyToClient.Character.unionId {replyToClient.character.unionId}");

            //Union myUnion = Server.Instances.GetInstance(replyToClient.Character.unionId) as Union;
            Union myUnion = server.database.SelectUnionById(replyToClient.character.unionId);
            _Logger.Debug($"my union is {myUnion.name}");

            IBuffer res5 = BufferProvider.Provide();

            res5.WriteUInt32(resultAcceptOrDeny); //Result
            res5.WriteUInt32(client.character.instanceId); //object id | Instance ID
            router.Send(replyToClient, (ushort)MsgPacketId.recv_union_reply_to_invite_r, res5, ServerType.Msg);

            if (resultAcceptOrDeny == 0)
            {
                client.character.unionId = myUnion.id;
                client.union = myUnion;
                client.union.Join(client);

                UnionMember myUnionMember = new UnionMember();
                server.instances.AssignInstance(myUnionMember);
                myUnionMember.unionId = myUnion.id;
                myUnionMember.characterDatabaseId = client.character.id;

                if (!server.database.InsertUnionMember(myUnionMember))
                {
                    _Logger.Error("union member could not be saved to database table nec_union_member");
                    return;
                }

                _Logger.Debug($"union member ID{myUnionMember.id} added to nec_union_member table");

                uint unionLeaderInstanceId = server.instances.GetCharacterInstanceId(myUnion.leaderId);
                uint unionSubLeader1InstanceId = server.instances.GetCharacterInstanceId(myUnion.subLeader1Id);
                uint unionSubLeader2InstanceId = server.instances.GetCharacterInstanceId(myUnion.subLeader2Id);

                TimeSpan difference = client.union.created.ToUniversalTime() - DateTime.UnixEpoch;
                int unionCreatedCalculation = (int)Math.Floor(difference.TotalSeconds);

                //Notify client if msg server found Union settings in database(memory) for client character Unique Persistant ID.
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(client.character.unionId); //Union Instance ID
                res.WriteFixedString(myUnion.name, 0x31); //size is 0x31
                res.WriteInt32(unionCreatedCalculation);
                res.WriteUInt32(unionLeaderInstanceId); //Leader
                res.WriteInt32(-1);
                res.WriteUInt32(unionSubLeader1InstanceId); //subleader1
                res.WriteInt32(-1);
                res.WriteUInt32(unionSubLeader2InstanceId); //subleader2
                res.WriteInt32(-1);
                res.WriteByte((byte)myUnion.level); //Union Level
                res.WriteUInt32(myUnion.currentExp); //Union EXP Current
                res.WriteUInt32(myUnion.nextLevelExp); //Union EXP next level Target
                res.WriteByte(myUnion
                    .memberLimitIncrease); //Increase Union Member Limit above default 50 (See Union Bonuses
                res.WriteByte(0);
                res.WriteUInt32(client.character.instanceId);
                res.WriteInt16(myUnion.capeDesignId); //Mantle/Cape design
                res.WriteFixedString($"You are all members of {myUnion.name} now.  Welcome!", 0x196); //size is 0x196
                for (int i = 0; i < 8; i++)
                    res.WriteInt32(i);
                res.WriteByte(0);

                router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail, res, ServerType.Msg);

                //Add all union members to your own instance of the union member list on your client
                foreach (UnionMember unionMemberList in server.database.SelectUnionMembersByUnionId(client.union.id))
                {
                    _Logger.Debug($"Loading union info for Member Id {unionMemberList.id}");
                    NecClient otherClient = server.clients.GetByCharacterId(unionMemberList.characterDatabaseId);
                    if (otherClient == null) continue;

                    Character character = otherClient.character;
                    if (character == null) continue;

                    Soul soul = otherClient.soul;
                    if (soul == null) continue;


                    _Logger.Debug($"character is named {character.name}");
                    _Logger.Debug($"Soul is named {soul.name}");
                    TimeSpan differenceJoined = unionMemberList.joined.ToUniversalTime() - DateTime.UnixEpoch;
                    int unionJoinedCalculation = (int)Math.Floor(differenceJoined.TotalSeconds);
                    IBuffer res3 = BufferProvider.Provide();
                    res3.WriteInt32(client.character.unionId); //Union Id
                    res3.WriteUInt32(character.instanceId);
                    res3.WriteFixedString($"{soul.name}", 0x31); //size is 0x31
                    res3.WriteFixedString($"{character.name}", 0x5B); //size is 0x5B
                    res3.WriteUInt32(character.classId);
                    res3.WriteByte(character.level);
                    res3.WriteInt32(character.mapId); // Location of your Union Member
                    res3.WriteInt32(0); //Area of Map, somehow. or Channel;
                    res3.WriteFixedString($"Channel {character.channel}", 0x61); // Channel location
                    res3.WriteUInt32(unionMemberList
                        .memberPriviledgeBitMask); //permissions bitmask  obxxxx1 = invite | obxxx1x = kick | obxx1xx = News | 0bxx1xxxxx = General Storage | 0bx1xxxxxx = Deluxe Storage
                    res3.WriteUInt32(unionMemberList.rank); //Rank  3 = beginner 2 = member, 1 = sub-leader 0 = leader
                    res3.WriteInt32(0); //online status. 0 = online, 1 = away, 2 = offline
                    res3.WriteInt32(unionJoinedCalculation); //Date Joined in seconds since unix time
                    res3.WriteInt32(Util.GetRandomNumber(0, 3));
                    res3.WriteInt32(Util.GetRandomNumber(0, 3));
                    router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail_member, res3, ServerType.Msg);
                }

                TimeSpan differenceInviteted = DateTime.Now - DateTime.UnixEpoch;
                int unionInvitedCalculation = (int)Math.Floor(differenceInviteted.TotalSeconds);

                //add you to all the member list of your union mates that were logged in when you joined.
                IBuffer res4 = BufferProvider.Provide();
                ;
                res4.WriteInt32(client.character.unionId); //not sure what this is.  union_Notify ID?
                res4.WriteUInt32(client.character.instanceId);
                res4.WriteFixedString($"{client.soul.name}", 0x31); //size is 0x31
                res4.WriteFixedString($"{client.character.name}", 0x5B); //size is 0x5B
                res4.WriteUInt32(client.character.classId);
                res4.WriteByte(client.character.level);
                res4.WriteInt32(client.character.mapId); // Location of your Union Member
                res4.WriteInt32(0); //Area of Map, somehow. or Channel;
                res4.WriteFixedString($"Channel {client.character.channel}", 0x61); // Channel location
                res4.WriteInt32(
                    0b11111111); //permissions bitmask  obxxxx1 = invite | obxxx1x = kick | obxx1xx = News | 0bxx1xxxxx = General Storage | 0bx1xxxxxx = Deluxe Storage
                res4.WriteInt32(3); //Rank  3 = beginner 2 = member, 1 = sub-leader 0 = leader
                res4.WriteInt32(0); //online status. 0 = online, 1 = away, 2 = offline
                res4.WriteInt32(unionInvitedCalculation); //Date Joined in seconds since unix time
                res4.WriteInt32(Util.GetRandomNumber(0, 3));
                res4.WriteInt32(Util.GetRandomNumber(0, 3));

                router.Send(client.union.unionMembers, (ushort)MsgPacketId.recv_union_notify_detail_member, res4,
                    ServerType.Msg, client);

                IBuffer res36 = BufferProvider.Provide();
                res36.WriteUInt32(client.character.instanceId);
                res36.WriteInt32(client.character.unionId);
                res36.WriteCString(myUnion.name);
                router.Send(client.map, (ushort)AreaPacketId.recv_chara_notify_union_data, res36, ServerType.Area);
            }
        }
    }
}
