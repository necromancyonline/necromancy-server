using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionStealUnidentified : PacketResponse
    {
        private readonly int _objectId;
        public RecvBattleReportActionStealUnidentified(int objectId)
            : base((ushort) AreaPacketId.recv_battle_report_action_steal_unidentified, ServerType.Area)
        {
            _objectId = objectId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); //Result

            res.WriteInt32(_objectId); //objectID

            res.WriteCString("ToBeFound"); // name
            return res;
        }
    }
}
