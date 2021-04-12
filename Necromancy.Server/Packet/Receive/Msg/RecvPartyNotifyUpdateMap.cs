using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyUpdateMap : PacketResponse
    {
        private readonly NecClient _client;
        private readonly Map _map;

        public RecvPartyNotifyUpdateMap(NecClient client)
            : base((ushort)MsgPacketId.recv_party_notify_update_map, ServerType.Msg)
        {
            _client = client;
            _map = client.map;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_client.character.instanceId); //character instance Id that moved maps
            res.WriteInt32(_client.character.mapId); //Map Serial ID
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteCString("Channel Awesome!");
            res.WriteInt32(Util.GetRandomNumber(0, 10));
            res.WriteByte((byte)Util.GetRandomNumber(0, 100));
            return res;
        }
    }
}
