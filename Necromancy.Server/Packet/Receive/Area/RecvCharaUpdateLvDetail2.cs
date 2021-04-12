using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateLvDetail2 : PacketResponse
    {
        private readonly Character _character;
        private readonly Experience _experience;

        public RecvCharaUpdateLvDetail2(Character character, Experience experience)
            : base((ushort)AreaPacketId.recv_chara_update_lv_detail2, ServerType.Area)
        {
            _character = character;
            _experience = experience;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(_character.level); // level
            res.WriteUInt64(_experience.CalculateLevelUp((uint)_character.level - 1).cumulativeExperience); // exp start
            res.WriteUInt64(_experience.CalculateLevelUp((uint)_character.level - 0).cumulativeExperience); // exp next
            res.WriteUInt64(_experience.CalculateLevelUp((uint)_character.level + 1).cumulativeExperience); // exp next 2
            return res;
        }
    }
}
