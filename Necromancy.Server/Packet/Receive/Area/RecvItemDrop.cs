using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemDrop : PacketResponse
    {
        private readonly int _error;

        public RecvItemDrop(NecClient client, int error)
            : base((ushort)AreaPacketId.recv_item_drop_r, ServerType.Area)
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
