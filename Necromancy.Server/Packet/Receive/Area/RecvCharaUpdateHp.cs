using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateHp : PacketResponse
    {
        private int _currentHp;

        public RecvCharaUpdateHp(int currentHp)
            : base((ushort)AreaPacketId.recv_chara_update_hp, ServerType.Area)
        {
            _currentHp = currentHp;
        }

        protected override IBuffer ToBuffer()
        {
            if (_currentHp < 0) _currentHp = 0;

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteInt32(_currentHp);
            return res7;
        }
    }
}
