using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateAc : PacketResponse
    {
        private readonly int _ac;

        public RecvCharaUpdateAc(int ac)
            : base((ushort)AreaPacketId.recv_chara_update_ac, ServerType.Area)
        {
            _ac = ac;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_ac);
            return res;
        }
    }
}
