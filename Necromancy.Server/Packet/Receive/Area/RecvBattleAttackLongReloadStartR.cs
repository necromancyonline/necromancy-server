using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleAttackLongReloadStartR : PacketResponse
    {
        public RecvBattleAttackLongReloadStartR()
            : base((ushort) AreaPacketId.recv_battle_attack_long_reload_start_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFloat(0);
            return res;
        }
    }
}
