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
            res.WriteInt16((short)_character.strength); //base Phys Attack
            res.WriteInt16((short)_character.dexterity); //base Phys Def
            res.WriteInt16((short)_character.intelligence); //base Mag attack
            res.WriteInt16((short)_character.piety); //base Mag Def

            res.WriteInt16((short)_character.luck); //Ranged physical attack

            res.WriteInt16(_battleParam.plusPhysicalAttack); //Equip Bonus Phys attack
            res.WriteInt16(_battleParam.plusPhysicalDefence); //Equip bonus Phys Def
            res.WriteInt16(_battleParam.plusMagicalAttack); //Equip Bonus Mag Attack
            res.WriteInt16(_battleParam.plusMagicalDefence); //Equip bonus Mag Def

            res.WriteInt16(_battleParam.plusRangedAttack); //Ranged physical attack
            return res;
        }
    }
}
