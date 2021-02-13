using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateWeight : PacketResponse
    {
        private int _weight;
        public RecvCharaUpdateWeight(int weight)
            : base((ushort) AreaPacketId.recv_chara_update_weight, ServerType.Area)
        {
            _weight = weight;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_weight);
            return res;
        }
    }
}
