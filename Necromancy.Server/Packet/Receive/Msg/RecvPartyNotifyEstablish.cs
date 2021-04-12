using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyEstablish : PacketResponse
    {
        private NecClient _client;
        private int _i;
        private readonly Party _party;

        public RecvPartyNotifyEstablish(NecClient client, Party party)
            : base((ushort)MsgPacketId.recv_party_notify_establish, ServerType.Msg)
        {
            _client = client;
            _party = party;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_party.instanceId); //Party Instance ID
            res.WriteInt32(_party.partyType); //Party type; 0 = closed, 1 = open.
            res.WriteInt32(_party.normalItemDist); //Normal item distribution; 0 = do not distribute, 1 = random.
            res.WriteInt32(_party.rareItemDist); //Rare item distribution; 0 = do not distribute, 1 = Draw.
            res.WriteInt32(1001010);
            res.WriteUInt32(1001010); //From player instance ID (but doesn't work?)
            foreach (NecClient client in _party.partyMembers)
            {
                res.WriteInt32(Util.GetRandomNumber(1, 10));
                res.WriteUInt32(client.character.instanceId); //Instance Id
                res.WriteFixedString($"{client.soul.name}", 0x31); //Soul name
                res.WriteFixedString($"{client.character.name}", 0x5B); //Chara name
                res.WriteUInt32(client.character.classId); //Class
                res.WriteByte(client.character.level); //Level
                res.WriteByte(client.character.criminalState); //Criminal state
                res.WriteByte(0); //Beginner Protection (bool)
                res.WriteByte(0); //Membership Status
                res.WriteByte((byte)Util.GetRandomNumber(1, 10));
                res.WriteByte((byte)Util.GetRandomNumber(1, 10)); //new JP
                _i++;
            }

            while (_i < 6)
            {
                res.WriteInt32(_i + 1);
                res.WriteUInt32(0); //Instance Id?
                res.WriteFixedString("", 0x31); //Soul name
                res.WriteFixedString("", 0x5B); //Chara name
                res.WriteUInt32(0); //Class
                res.WriteByte(0); //Level
                res.WriteByte(0); //Criminal state
                res.WriteByte(0); //Beginner Protection (bool)
                res.WriteByte(0); //Membership Status
                res.WriteByte(0);
                res.WriteByte(0); //new JP
                _i++;
            }

            res.WriteByte((byte)_party.partyMembers.Count); // number of above party member entries to display in invite
            res.WriteByte(0 /*party.Mentoring*/); //Bool Mentoring. 1 On 0 Off
            return res;
        }
    }
}
