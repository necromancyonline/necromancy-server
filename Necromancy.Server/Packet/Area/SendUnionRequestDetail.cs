using System;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendUnionRequestDetail : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendUnionRequestDetail));

        public SendUnionRequestDetail(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_union_request_detail;

        public override void Handle(NecClient client, NecPacket packet)
        {
            UnionMember unionMember = server.database.SelectUnionMemberByCharacterId(client.character.id);
            if (unionMember == null)
            {
                _Logger.Debug($"you don't appear to be in a union");
            }
            else
            {
                _Logger.Debug($"union member ID{unionMember.id} found. loading Union information");
                Union myUnion = server.database.SelectUnionById(unionMember.unionId);

                if (myUnion == null)
                {
                    _Logger.Error($"This is Strange.. Can't find a Union with id {unionMember.unionId}");
                }
                else
                {
                    _Logger.Debug($"union  ID{unionMember.unionId} found. continuing loading of Union information");

                    client.character.unionId = myUnion.id;
                    client.union = myUnion;
                    client.union.Join(client);

                    TimeSpan differenceCreated = client.union.created.ToUniversalTime() - DateTime.UnixEpoch;
                    int unionCreatedCalculation = (int)Math.Floor(differenceCreated.TotalSeconds);

                    //To-Do,  move this whole thing to a response packet, since it's used multiple places.
                    //Notify client of each union member in above union, queried by charaname and InstanceId (for menu based interactions)
                    foreach (UnionMember unionMemberList in server.database.SelectUnionMembersByUnionId(client.character
                        .unionId))
                    {
                        int onlineStatus = 1;
                        Character character = null;
                        Soul soul = null;
                        NecClient otherClient = server.clients.GetByCharacterId(unionMemberList.characterDatabaseId);
                        if (otherClient == null)
                        {
                            character = server.instances.GetCharacterByDatabaseId(unionMemberList.characterDatabaseId);
                            soul = server.database.SelectSoulById(character.soulId);
                        }
                        else
                        {
                            character = otherClient.character;
                            soul = otherClient.soul;
                            onlineStatus = 0;
                        }
                        if (character == null)
                        {
                            continue;
                        }
                        if (soul == null)
                        {
                            continue;
                        }

                        TimeSpan differenceJoined = unionMemberList.joined.ToUniversalTime() - DateTime.UnixEpoch;
                        int unionJoinedCalculation = (int)Math.Floor(differenceJoined.TotalSeconds);
                        IBuffer res3 = BufferProvider.Provide();
                        res3.WriteInt32(client.character.unionId); //Union Id
                        res3.WriteUInt32(character.instanceId);  //unique Identifier of the member in the union roster.
                        res3.WriteFixedString($"{soul.name}", 0x31); //size is 0x31
                        res3.WriteFixedString($"{character.name}", 0x5B); //size is 0x5B
                        res3.WriteUInt32(character.classId);
                        res3.WriteByte(character.level);
                        res3.WriteByte(0);//new

                        res3.WriteInt32(character.mapId); // Location of your Union Member
                        res3.WriteInt32(unionJoinedCalculation); //Area of Map, somehow. or Channel;
                        res3.WriteFixedString($"Channel {character.channel}", 0x61); // Channel location
                        res3.WriteUInt32(unionMemberList.memberPriviledgeBitMask); //permissions bitmask  obxxxx1 = invite | obxxx1x = kick | obxx1xx = News | 0bxx1xxxxx = General Storage | 0bx1xxxxxx = Deluxe Storage
                        res3.WriteByte(0);//new
                        res3.WriteUInt32(unionMemberList.rank); //Rank  3 = beginner 2 = member, 1 = sub-leader 0 = leader
                        res3.WriteInt32(onlineStatus); //online status. 0 = online, 1 = offline, 2 = away
                        res3.WriteInt32(unionJoinedCalculation); //Date Joined in seconds since unix time
                        res3.WriteInt32(Util.GetRandomNumber(0, 0));
                        res3.WriteInt32(Util.GetRandomNumber(0, 0));
                        res3.WriteInt32(Util.GetRandomNumber(0, 0));//new
                        res3.WriteFixedString($"{character.name}", 0x181); //size is 0x181, new

                        router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail_member, res3, ServerType.Msg);
                    }

                    uint unionLeaderInstanceId = server.instances.GetCharacterInstanceId(myUnion.leaderId);
                    uint unionSubLeader1InstanceId = server.instances.GetCharacterInstanceId(myUnion.subLeader1Id);
                    uint unionSubLeader2InstanceId = server.instances.GetCharacterInstanceId(myUnion.subLeader2Id);

                    //Notify client if msg server found Union settings in database(memory) for client character Unique Persistant ID.
                    IBuffer res = BufferProvider.Provide();
                    res.WriteInt32(unionMember.unionId); //Union Instance ID //form the ToDo Logic above
                    res.WriteFixedString(myUnion.name, 0x31); //size is 0x31
                    res.WriteInt32(unionCreatedCalculation); //Creation Date in seconds since unix 0 time (Jan. 1, 1970)
                    res.WriteUInt32(unionLeaderInstanceId); //Leader
                    res.WriteInt32(unionCreatedCalculation);//Last login timestamp for demoting?
                    res.WriteUInt32(unionSubLeader1InstanceId); //subleader1
                    res.WriteInt32(unionCreatedCalculation);//Last login timestamp for demoting?
                    res.WriteUInt32(unionSubLeader2InstanceId); //subleader2
                    res.WriteInt32(unionCreatedCalculation);//Last login timestamp for demoting?
                    res.WriteByte((byte)myUnion.level); //Union Level
                    res.WriteUInt32(myUnion.currentExp); //Union EXP Current
                    res.WriteUInt32(myUnion.nextLevelExp); //Union EXP next level Target
                    res.WriteByte(myUnion.memberLimitIncrease); //Increase Union Member Limit above default 50 (See Union Bonuses
                    res.WriteByte(1); //?
                    res.WriteInt32(10); //Creation Date?
                    res.WriteInt16(myUnion.capeDesignId); //Mantle/Cape
                    res.WriteFixedString($"You are all members of {myUnion.name} now.  Welcome!",
                        0x196); //size is 0x196
                    for (int i = 0; i < 8; i++)
                        res.WriteInt32(i);
                    res.WriteByte(255);
                    res.WriteInt32(0);

                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);

                    router.Send(client, (ushort)MsgPacketId.recv_union_notify_detail, res, ServerType.Msg);
                }
            }

                //Acknowledge send.  'Hey send,  i'm finished doing my stuff.  go do the next stuff'
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt32(0);
                router.Send(client, (ushort)MsgPacketId.recv_union_request_detail_r, res2, ServerType.Msg);

        }
    }
}
