using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionItemUse : PacketResponse
    {
        private readonly int _objectId;

        public RecvBattleReportActionItemUse(int objectId)
            : base((ushort) AreaPacketId.recv_battle_report_action_item_use, ServerType.Area)
        {
            _objectId = objectId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_objectId);
            return res;
        }
    }
}
