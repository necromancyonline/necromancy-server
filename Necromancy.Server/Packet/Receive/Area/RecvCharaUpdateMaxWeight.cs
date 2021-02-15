using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateMaxWeight : PacketResponse
    {
        private int _max;
        private int _diff;
        public RecvCharaUpdateMaxWeight(int max, int diff)
            : base((ushort) AreaPacketId.recv_chara_update_maxweight, ServerType.Area)
        {
            _max = max;
            _diff = diff;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_max);
            res.WriteInt32(_diff);
            return res;
        }
    }
}
