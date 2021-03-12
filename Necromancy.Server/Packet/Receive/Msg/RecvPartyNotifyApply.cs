using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyApply : PacketResponse
    {
        private NecClient _client;
        public RecvPartyNotifyApply(NecClient client)
            : base((ushort) MsgPacketId.recv_party_notify_apply, ServerType.Msg)
        {
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_client.Character.partyId); //Party ID?
            res.WriteUInt32(_client.Character.InstanceId);
            res.WriteFixedString($"{_client.Soul.Name}", 0x31);
            res.WriteFixedString($"{_client.Character.Name}", 0x5B);
            res.WriteUInt32(_client.Character.ClassId);
            res.WriteByte(_client.Character.Level);
            res.WriteByte(_client.Soul.Level);
            res.WriteByte((byte)(_client.Character.criminalState+5)); //Criminal Status
            res.WriteByte(0); //Beginner Protection (bool) 
            res.WriteByte(3); //Membership Status
            res.WriteByte(0);
            return res;
        }
    }
}
