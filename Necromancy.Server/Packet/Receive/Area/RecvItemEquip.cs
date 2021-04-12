using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemEquip : PacketResponse
    {
        private readonly int _error;

        public RecvItemEquip(NecClient necClient, int error)
            : base((ushort)AreaPacketId.recv_item_equip_r, ServerType.Area)
        {
            _error = error;
            Clients.Add(necClient);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_error);
            return res;
        }
    }
}
