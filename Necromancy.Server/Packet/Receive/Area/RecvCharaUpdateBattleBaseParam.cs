using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Stats;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateBattleBaseParam : PacketResponse
    {
        private Character _character;
        private BattleParam _battleParam;
        public RecvCharaUpdateBattleBaseParam(Character character, BattleParam battleParam)
            : base((ushort) AreaPacketId.recv_chara_update_battle_base_param, ServerType.Area)
        {
            _character = character;
            _battleParam = battleParam;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16((short)_character.Strength); //base Phys Attack
            res.WriteInt16((short)_character.Dexterity); //base Phys Def
            res.WriteInt16((short)_character.Intelligence); //base Mag attack
            res.WriteInt16((short)_character.Piety); //base Mag Def

            res.WriteInt16((short)_character.Luck); //Ranged physical attack

            res.WriteInt16(_battleParam.PlusPhysicalAttack); //Equip Bonus Phys attack
            res.WriteInt16(_battleParam.PlusPhysicalDefence); //Equip bonus Phys Def
            res.WriteInt16(_battleParam.PlusMagicalAttack); //Equip Bonus Mag Attack
            res.WriteInt16(_battleParam.PlusMagicalDefence); //Equip bonus Mag Def

            res.WriteInt16(_battleParam.PlusRangedAttack); //Ranged physical attack
            return res;
        }
    }
}
