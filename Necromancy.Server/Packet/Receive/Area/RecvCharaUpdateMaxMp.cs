using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateMaxMp : PacketResponse
    {
        private readonly int _mp;

        public RecvCharaUpdateMaxMp(int mp)
            : base((ushort)AreaPacketId.recv_chara_update_maxmp, ServerType.Area)
        {
            _mp = mp;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_mp);
            return res;
        }
    }
}
