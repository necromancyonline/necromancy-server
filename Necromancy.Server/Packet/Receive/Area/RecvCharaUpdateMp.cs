using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateMp : PacketResponse
    {
        private readonly int _mp;

        public RecvCharaUpdateMp(int mp)
            : base((ushort)AreaPacketId.recv_chara_update_mp, ServerType.Area)
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
