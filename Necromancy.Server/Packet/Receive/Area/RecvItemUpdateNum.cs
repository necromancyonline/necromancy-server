using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateNum : PacketResponse
    {
        private readonly ItemInstance _item;
        public RecvItemUpdateNum(NecClient client, ItemInstance item)
            : base((ushort) AreaPacketId.recv_item_update_num, ServerType.Area)
        {
            _item = item;
            clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_item.instanceId);
            res.WriteByte(_item.quantity);
            return res;
        }
    }
}
