using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyInvite : PacketResponse
    {
        private NecClient _client;
        private Party _party;
        private int i = 0;
        public RecvPartyNotifyInvite(NecClient client, Party party)
            : base((ushort)MsgPacketId.recv_party_notify_invite, ServerType.Msg)
        {
            _client = client;
            _party = party;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_party.InstanceId); //Party Instance ID
            res.WriteInt32(_party.PartyType); //Party type; 0 = closed, 1 = open.
            res.WriteInt32(_party.NormalItemDist); //Normal item distribution; 0 = do not distribute, 1 = random.
            res.WriteInt32(_party.RareItemDist); //Rare item distribution; 0 = do not distribute, 1 = Draw.
            res.WriteUInt32(_client.Character.InstanceId);
            res.WriteUInt32(_party.PartyLeaderId); //From player instance ID (but doesn't work?)
            foreach (NecClient client in _party.PartyMembers)
            //for (int i = 0; i < 6; i++)
            {
                res.WriteInt32(i+1);
                res.WriteUInt32(client.Character.InstanceId); //Instance Id?
                res.WriteFixedString($"{client.Soul.Name}", 0x31); //Soul name
                res.WriteFixedString($"{client.Character.Name}", 0x5B); //Chara name
                res.WriteUInt32(client.Character.ClassId); //Class
                res.WriteByte(client.Character.Level); //Level
                res.WriteByte(client.Character.criminalState); //Criminal state
                res.WriteByte(0); //Beginner Protection (bool) 
                res.WriteByte(0); //Membership Status
                res.WriteByte(0);
                res.WriteByte(0); //new JP
                i++;
            }
            while (i < 6)
            {
                res.WriteInt32(i+1);
                res.WriteUInt32(0); //Instance Id?
                res.WriteFixedString($"", 0x31); //Soul name
                res.WriteFixedString($"", 0x5B); //Chara name
                res.WriteUInt32(0); //Class
                res.WriteByte(0); //Level
                res.WriteByte(0); //Criminal state
                res.WriteByte(0); //Beginner Protection (bool) 
                res.WriteByte(0); //Membership Status
                res.WriteByte(0);
                res.WriteByte(0); //new JP
                i++;
            }

            res.WriteByte((byte)_party.PartyMembers.Count); // number of above party member entries to display in invite
            res.WriteByte(1); //Bool
            res.WriteFixedString($"Hey Join my Party", 0xB5); //size is 0xB5
            return res;
        }
    }
}
