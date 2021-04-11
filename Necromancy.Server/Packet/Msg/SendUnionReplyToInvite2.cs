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
    public class send_union_reply_to_invite2 : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_union_reply_to_invite2));

        public send_union_reply_to_invite2(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_reply_to_invite2;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint replyToInstanceId = packet.Data.ReadUInt32();
            uint resultAcceptOrDeny = packet.Data.ReadUInt32();
            NecClient replyToClient = Server.Clients.GetByCharacterInstanceId(replyToInstanceId);
            Logger.Debug(
                $"replyToInstanceId {replyToInstanceId} resultAcceptOrDeny {resultAcceptOrDeny} replyToClient.Character.unionId {replyToClient.Character.unionId}");

            //Union myUnion = Server.Instances.GetInstance(replyToClient.Character.unionId) as Union;
            Union myUnion = Server.Database.SelectUnionById(replyToClient.Character.unionId);
            Logger.Debug($"my union is {myUnion.Name}");

            IBuffer res5 = BufferProvider.Provide();

            res5.WriteUInt32(resultAcceptOrDeny); //Result
            res5.WriteUInt32(client.Character.InstanceId); //object id | Instance ID
            Router.Send(replyToClient, (ushort) MsgPacketId.recv_union_reply_to_invite_r, res5, ServerType.Msg);

            if (resultAcceptOrDeny == 0)
            {
                client.Character.unionId = myUnion.Id;
                client.Union = myUnion;
                client.Union.Join(client);

                UnionMember myUnionMember = new UnionMember();
                Server.Instances.AssignInstance(myUnionMember);
                myUnionMember.UnionId = (int) myUnion.Id;
                myUnionMember.CharacterDatabaseId = client.Character.Id;

                if (!Server.Database.InsertUnionMember(myUnionMember))
                {
                    Logger.Error($"union member could not be saved to database table nec_union_member");
                    return;
                }

                Logger.Debug($"union member ID{myUnionMember.Id} added to nec_union_member table");

                uint UnionLeaderInstanceId = Server.Instances.GetCharacterInstanceId(myUnion.LeaderId);
                uint UnionSubLeader1InstanceId = Server.Instances.GetCharacterInstanceId(myUnion.SubLeader1Id);
                uint UnionSubLeader2InstanceId = Server.Instances.GetCharacterInstanceId(myUnion.SubLeader2Id);

                TimeSpan difference = client.Union.Created.ToUniversalTime() - DateTime.UnixEpoch;
                int unionCreatedCalculation = (int) Math.Floor(difference.TotalSeconds);

                //Notify client if msg server found Union settings in database(memory) for client character Unique Persistant ID.
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(client.Character.unionId); //Union Instance ID
                res.WriteFixedString(myUnion.Name, 0x31); //size is 0x31
                res.WriteInt32(unionCreatedCalculation);
                res.WriteUInt32(UnionLeaderInstanceId); //Leader
                res.WriteInt32(-1);
                res.WriteUInt32(UnionSubLeader1InstanceId); //subleader1
                res.WriteInt32(-1);
                res.WriteUInt32(UnionSubLeader2InstanceId); //subleader2
                res.WriteInt32(-1);
                res.WriteByte((byte) myUnion.Level); //Union Level
                res.WriteUInt32(myUnion.CurrentExp); //Union EXP Current
                res.WriteUInt32(myUnion.NextLevelExp); //Union EXP next level Target
                res.WriteByte(myUnion
                    .MemberLimitIncrease); //Increase Union Member Limit above default 50 (See Union Bonuses
                res.WriteByte(0);
                res.WriteUInt32(client.Character.InstanceId);
                res.WriteInt16(myUnion.CapeDesignID); //Mantle/Cape design
                res.WriteFixedString($"You are all members of {myUnion.Name} now.  Welcome!", 0x196); //size is 0x196
                for (int i = 0; i < 8; i++)
                    res.WriteInt32(i);
                res.WriteByte(0);

                Router.Send(client, (ushort) MsgPacketId.recv_union_notify_detail, res, ServerType.Msg);

                //Add all union members to your own instance of the union member list on your client
                foreach (UnionMember unionMemberList in Server.Database.SelectUnionMembersByUnionId(client.Union.Id))
                {
                    Logger.Debug($"Loading union info for Member Id {unionMemberList.Id}");
                    NecClient otherClient = Server.Clients.GetByCharacterId(unionMemberList.CharacterDatabaseId);
                    if (otherClient == null)
                    {
                        continue;
                    }

                    Character character = otherClient.Character;
                    if (character == null)
                    {
                        continue;
                    }

                    Soul soul = otherClient.Soul;
                    if (soul == null)
                    {
                        continue;
                    }


                    Logger.Debug($"character is named {character.Name}");
                    Logger.Debug($"Soul is named {soul.Name}");
                    TimeSpan differenceJoined = unionMemberList.Joined.ToUniversalTime() - DateTime.UnixEpoch;
                    int unionJoinedCalculation = (int) Math.Floor(differenceJoined.TotalSeconds);
                    IBuffer res3 = BufferProvider.Provide();
                    res3.WriteInt32(client.Character.unionId); //Union Id
                    res3.WriteUInt32(character.InstanceId);
                    res3.WriteFixedString($"{soul.Name}", 0x31); //size is 0x31
                    res3.WriteFixedString($"{character.Name}", 0x5B); //size is 0x5B
                    res3.WriteUInt32(character.ClassId);
                    res3.WriteByte(character.Level);
                    res3.WriteInt32(character.MapId); // Location of your Union Member
                    res3.WriteInt32(0); //Area of Map, somehow. or Channel;
                    res3.WriteFixedString($"Channel {character.Channel}", 0x61); // Channel location
                    res3.WriteUInt32(unionMemberList
                        .MemberPriviledgeBitMask); //permissions bitmask  obxxxx1 = invite | obxxx1x = kick | obxx1xx = News | 0bxx1xxxxx = General Storage | 0bx1xxxxxx = Deluxe Storage
                    res3.WriteUInt32(unionMemberList.Rank); //Rank  3 = beginner 2 = member, 1 = sub-leader 0 = leader
                    res3.WriteInt32(0); //online status. 0 = online, 1 = away, 2 = offline
                    res3.WriteInt32(unionJoinedCalculation); //Date Joined in seconds since unix time
                    res3.WriteInt32(Util.GetRandomNumber(0, 3));
                    res3.WriteInt32(Util.GetRandomNumber(0, 3));
                    Router.Send(client, (ushort) MsgPacketId.recv_union_notify_detail_member, res3, ServerType.Msg);
                }

                TimeSpan differenceInviteted = DateTime.Now - DateTime.UnixEpoch;
                int unionInvitedCalculation = (int) Math.Floor(differenceInviteted.TotalSeconds);

                //add you to all the member list of your union mates that were logged in when you joined.
                IBuffer res4 = BufferProvider.Provide();
                ;
                res4.WriteInt32(client.Character.unionId); //not sure what this is.  union_Notify ID?
                res4.WriteUInt32(client.Character.InstanceId);
                res4.WriteFixedString($"{client.Soul.Name}", 0x31); //size is 0x31
                res4.WriteFixedString($"{client.Character.Name}", 0x5B); //size is 0x5B
                res4.WriteUInt32(client.Character.ClassId);
                res4.WriteByte(client.Character.Level);
                res4.WriteInt32(client.Character.MapId); // Location of your Union Member
                res4.WriteInt32(0); //Area of Map, somehow. or Channel;
                res4.WriteFixedString($"Channel {client.Character.Channel}", 0x61); // Channel location
                res4.WriteInt32(
                    0b11111111); //permissions bitmask  obxxxx1 = invite | obxxx1x = kick | obxx1xx = News | 0bxx1xxxxx = General Storage | 0bx1xxxxxx = Deluxe Storage
                res4.WriteInt32(3); //Rank  3 = beginner 2 = member, 1 = sub-leader 0 = leader
                res4.WriteInt32(0); //online status. 0 = online, 1 = away, 2 = offline
                res4.WriteInt32(unionInvitedCalculation); //Date Joined in seconds since unix time
                res4.WriteInt32(Util.GetRandomNumber(0, 3));
                res4.WriteInt32(Util.GetRandomNumber(0, 3));

                Router.Send(client.Union.UnionMembers, (ushort) MsgPacketId.recv_union_notify_detail_member, res4,
                    ServerType.Msg, client);

                IBuffer res36 = BufferProvider.Provide();
                res36.WriteUInt32(client.Character.InstanceId);
                res36.WriteInt32(client.Character.unionId);
                res36.WriteCString(myUnion.Name);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_notify_union_data, res36, ServerType.Area);
            }
        }
    }
}
