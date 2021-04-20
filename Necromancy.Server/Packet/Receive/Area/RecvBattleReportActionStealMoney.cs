using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionStealMoney : PacketResponse
    {
        private readonly int _objectId;

        public RecvBattleReportActionStealMoney(int objectId)
            : base((ushort)AreaPacketId.recv_battle_report_action_steal_money, ServerType.Area)
        {
            _objectId = objectId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //result
            res.WriteInt32(_objectId); //objectId
            res.WriteInt32(12345); //money
            return res;
        }
    }
}
