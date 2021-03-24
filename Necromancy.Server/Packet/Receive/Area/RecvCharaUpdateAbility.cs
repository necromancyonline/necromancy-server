using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateAbility : PacketResponse
    {
        public enum ability : int
        {
            _str = 0,
            _vit = 1,
            _dex = 2,
            _agi = 3,
            _int = 4,
            _pie = 5,
            _luk = 6
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
