using System;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendUnionRequestNews : ClientHandler
    {
        public SendUnionRequestNews(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_union_request_news;


        public override void Handle(NecClient client, NecPacket packet)
        {
            int newsEntries = 25; /*myUnion.NewsEntries.Count();*/ //max of 0x3E8
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0 /*myUnion.UnionNewsList.InstanceID*/); // News list instance ID?
            res.WriteInt32(newsEntries); //less than or equal to 0x3E8
            for (int i = 0; i < newsEntries; i++) //limit is the int32 above
            {
                res.WriteInt32(10000 + i); //News entry ID
                res.WriteFixedString($"{client.soul.name}", 0x31); //soul name
                res.WriteFixedString($"{client.character.name}", 0x5B); //character name
                res.WriteInt32(i); // Activity lookup from Str_table.csv A=100 B=745 c=0 to 17.
                res.WriteFixedString($"{i} bottles of beer on the wall", 0x49); //Parameter 3 for str table
                res.WriteFixedString($"Pass it around. {i - 1} bottles of Beer on the wall", 0x49); //unknown
                res.WriteInt32(i - 1); //count of items or gold being actioned. parameter 4 for str table
            }

            router.Send(client, (ushort) MsgPacketId.recv_union_request_news_r, res, ServerType.Msg);


            //Query and update the state of all members in your union roster. for when you click the members tab
            foreach (UnionMember unionMemberList in server.database.SelectUnionMembersByUnionId(client.character.unionId))
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

                res = BufferProvider.Provide();

                res.WriteInt32(client.character.unionId); //Union Id
                res.WriteUInt32(character.instanceId);
                res.WriteFixedString($"{soul.name}", 0x31); //size is 0x31
                res.WriteFixedString($"{character.name}", 0x5B); //size is 0x5B
                res.WriteUInt32(character.classId);
                res.WriteByte(character.level);
                res.WriteByte(0);//new
                res.WriteInt32(character.mapId); // Location of your Union Member
                res.WriteInt32(69); //Area of Map, somehow. or Channel;
                res.WriteFixedString($"Channel {character.channel}", 0x61); // Channel location
                res.WriteUInt32(unionMemberList.memberPriviledgeBitMask); //permissions bitmask  obxxxx1 = invite | obxxx1x = kick | obxx1xx = News | 0bxx1xxxxx = General Storage | 0bx1xxxxxx = Deluxe Storage
                res.WriteByte(0);//new
                res.WriteUInt32(unionMemberList.rank); //Rank  3 = beginner 2 = member, 1 = sub-leader 0 = leader
                res.WriteInt32(onlineStatus); //online status. 0 = online, 1 = offline, 2 = away
                res.WriteInt32(69); //Date Joined in seconds since unix time
                res.WriteInt32(Util.GetRandomNumber(0, 0));
                res.WriteInt32(Util.GetRandomNumber(0, 0));
                res.WriteInt32(0);//new
                res.WriteFixedString("", 0x181); //size is 0x181

                router.Send(client, (ushort)MsgPacketId.recv_union_notify_member_state, res, ServerType.Msg);
            }


        }
    }
}
