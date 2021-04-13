using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopSell : PacketResponse
    {
        private readonly int _error;

        public RecvShopSell(NecClient client, int error)
            : base((ushort)AreaPacketId.recv_shop_sell_r, ServerType.Area)
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
