using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateLvDetail : PacketResponse
    {
        Character _character;
        Experience _experience;
        public RecvCharaUpdateLvDetail(Character character, Experience experience)
            : base((ushort) AreaPacketId.recv_chara_update_lv_detail, ServerType.Area)
        {
            _character = character;
            _experience = experience;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(_character.level); // new level

            res.WriteUInt64(_experience.CalculateLevelUp((uint)_character.level - 1).cumulativeExperience); // exp start
            res.WriteUInt64(_experience.CalculateLevelUp((uint)_character.level - 0).cumulativeExperience); // exp next
            return res;
        }
    }
}
