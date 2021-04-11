using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateAbility : PacketResponse
    {
        public enum Ability : int
        {
            Str = 0,
            Vit = 1,
            Dex = 2,
            Agi = 3,
            Int = 4,
            Pie = 5,
            Luk = 6
        }
        int _abilityNumber;
        ushort _abilityBase;
        ushort _abilityTotal;
        public RecvCharaUpdateAbility(int abilityNumber, ushort abilityBase, ushort abilityTotal)
            : base((ushort) AreaPacketId.recv_chara_update_ability, ServerType.Area)
        {
            _abilityNumber = abilityNumber;
            _abilityBase = abilityBase;
            _abilityTotal = (ushort)(abilityTotal + abilityBase);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_abilityNumber); //attribute
            res.WriteUInt16(_abilityBase); //base
            res.WriteUInt16(_abilityTotal); //total
            return res;
        }
    }
}
