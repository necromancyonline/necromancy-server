using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyAddMember : PacketResponse
    {
        private NecClient _client;
        public RecvPartyNotifyAddMember(NecClient client)
            : base((ushort) MsgPacketId.recv_party_notify_add_member, ServerType.Msg)
        {
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); //PARTY_ADD_REASON
            /*
            PARTY_ADD	0	%s is attending the party. 
            PARTY_ADD	1	%s has joined the party. 
            I participated in the party of PARTY_ADD	10	%s. 
            */

            res.WriteUInt32(_client.Character.DeadBodyInstanceId);//DeadBodyId?
            res.WriteUInt32(_client.Character.InstanceId); //Chara Instance Id
            res.WriteFixedString($"{_client.Soul.Name}", 0x31); //Soul name
            res.WriteFixedString($"{_client.Character.Name}", 0x5B); //Character name
            res.WriteUInt32(_client.Character.ClassId); //Class/job
            res.WriteByte(_client.Soul.Level); //Soul rank
            res.WriteByte(_client.Character.Level); //Character level
            res.WriteInt32(_client.Character.Hp.current); // current hp?
            res.WriteInt32(_client.Character.Mp.current); // current mp?
            res.WriteInt32(_client.Character.Od.current); // current od?
            res.WriteInt32(_client.Character.Hp.max); // max hp?
            res.WriteInt32(_client.Character.Mp.max); // maxmp?
            res.WriteInt32(_client.Character.Od.max); // max od?
            res.WriteInt32(_client.Character.MapId); //Might make the character selectable?
            res.WriteInt32(_client.Character.MapId); //One half of location? 1001902 = Illfalo Port but is actually Deep Sea Port
            res.WriteInt32(500); // current guard points?
            res.WriteInt32(600); // max guard points?
            res.WriteFixedString($"Channel {_client.Character.Channel}",0x61); //Location of player if not in same zone
            res.WriteInt32(3/*_client.Character.Ac*/); //AC? which is  like, chance to dodge
            res.WriteByte(3); // condition?
            res.WriteFloat(_client.Character.X);
            res.WriteFloat(_client.Character.Y);
            res.WriteFloat(_client.Character.Z);
            res.WriteByte(_client.Character.Heading);
            res.WriteByte((byte)(_client.Character.criminalState+5)); //?
            res.WriteByte(1); //Beginner Protection (bool) ??? Dragon (in boss fight) ??
            res.WriteByte(3); //Membership Status???

            return res;
        }
    }
}
