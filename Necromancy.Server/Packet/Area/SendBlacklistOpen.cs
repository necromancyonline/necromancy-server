using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_blacklist_open : ClientHandler
    {
        public send_blacklist_open(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_blacklist_open;

        public override void Handle(NecClient client, NecPacket packet)
        {

            TimeSpan differenceJoined = DateTime.Today.ToUniversalTime() - DateTime.UnixEpoch;
            int DateAttackedCalculation = (int)Math.Floor(differenceJoined.TotalSeconds);

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); //must be 0

            res.WriteInt32(10); //count of entries to display

            //for (int i = 0; i < 10; i++)
            int i = 0;
            foreach (Character blackCharacter in Server.Database.SelectCharacters()) //Max 10 Loops. will break if more than 10 characters in Db.
            {
                Soul blackSoul = Server.Database.SelectSoulById(blackCharacter.SoulId);
                int onlineStatus = 0;
                NecClient otherClient = Server.Clients.GetByCharacterId(blackCharacter.Id);
                if (otherClient == null)
                {
                    //character = Server.Instances.GetCharacterByDatabaseId(unionMemberList.CharacterDatabaseId);
                    //soul = Server.Database.SelectSoulById(character.SoulId);
                }
                else
                {
                    onlineStatus = 1;
                }

                res.WriteInt32(DateAttackedCalculation); //TimeStamp of when you were PKed or Stolen from
                res.WriteUInt32(blackCharacter.InstanceId); //
                res.WriteInt32(Util.GetRandomNumber(1, 10)); //Count of times BlackListMember PKed you
                res.WriteInt32(Util.GetRandomNumber(0, 3)); //count of times BlackListMember looted you
                res.WriteInt32(Util.GetRandomNumber(0, 150)); //Count of items BlackListMember looted from you
                res.WriteInt32(1); //Union ID?  we dont have any unions yet
                res.WriteByte((byte)Util.GetRandomNumber(0, 2)); //Locked entry on blacklist  1:yes 0:No
                res.WriteByte((byte)Util.GetRandomNumber(0,2)); // Current Bounty on blackCharacter.  1:Yes  0:No

                res.WriteInt32(blackSoul.Id); //Soul ID of BlackListMember for sending bounty to Area server
                res.WriteFixedString(blackSoul.Name, 49); //Soul Name of BlackListMember

                res.WriteInt32(onlineStatus); //online status. 0 = online, 1 = offline, 2 = away
                res.WriteFixedString(blackCharacter.Name, 91); //character name of BlackListMember
                res.WriteUInt32(blackCharacter.ClassId); // Character Class of BlackListMember
                res.WriteByte(blackCharacter.Level); //Character level of BlackListMember
                res.WriteInt32(blackCharacter.MapId); //Character Map ID of BlackListMember
                res.WriteInt32(blackCharacter.MapId); //world number?? or Map Area?
                res.WriteFixedString($"Channel {blackCharacter.Channel}", 97);
                i++;
                if (i == 10)
                    break;
            }

            Router.Send(client, (ushort)AreaPacketId.recv_blacklist_open_r, res, ServerType.Area);
        }
    }
}
