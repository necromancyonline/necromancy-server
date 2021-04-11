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
            res.WriteUInt32(_client.character.partyId); //Party ID?
            res.WriteUInt32(_client.character.instanceId);
            res.WriteFixedString($"{_client.soul.name}", 0x31);
            res.WriteFixedString($"{_client.character.name}", 0x5B);
            res.WriteUInt32(_client.character.classId);
            res.WriteByte(_client.character.level);
            res.WriteByte(_client.soul.level);
            res.WriteByte((byte)(_client.character.criminalState+5)); //Criminal Status
            res.WriteByte(0); //Beginner Protection (bool)
            res.WriteByte(3); //Membership Status
            res.WriteByte(0);
            return res;
        }
    }
}
