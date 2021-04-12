using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestGetRogueMissionQuestHistoryR : PacketResponse
    {
        public RecvQuestGetRogueMissionQuestHistoryR()
            : base((ushort)AreaPacketId.recv_quest_get_rogue_mission_quest_history_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(1);
            return res;
        }
    }
}
