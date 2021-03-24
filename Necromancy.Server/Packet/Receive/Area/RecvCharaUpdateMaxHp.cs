using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateMaxHp : PacketResponse
    {
        private int _hp;
        public RecvCharaUpdateMaxHp(int hp)
            : base((ushort) AreaPacketId.recv_chara_update_maxhp, ServerType.Area)
        {
            _hp = hp;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_hp);
            return res;
        }
    }
}
