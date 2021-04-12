using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyUpdateSyncLevel : PacketResponse
    {
        private readonly NecClient _client;

        public RecvPartyNotifyUpdateSyncLevel(NecClient client)
            : base((ushort)MsgPacketId.recv_party_notify_update_sync_level, ServerType.Msg)
        {
            _client = client;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_client.character.instanceId);
            res.WriteInt32(_client.character.level); //Level Limit
            return res;
        }
    }
}
