using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulDispItemRequestData : PacketResponse
    {
        private readonly int _error;
        public RecvSoulDispItemRequestData(NecClient client, int error)
            : base((ushort) AreaPacketId.recv_soul_dispitem_request_data_r, ServerType.Area)
        {
            _error = error;
            clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_error);
            return res;
        }
    }
}
