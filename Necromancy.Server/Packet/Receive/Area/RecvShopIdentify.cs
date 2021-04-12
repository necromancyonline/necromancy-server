using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopIdentify : PacketResponse
    {
        private readonly int _error;

        public RecvShopIdentify(NecClient client, int error)
            : base((ushort)AreaPacketId.recv_shop_identify_r, ServerType.Area)
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
