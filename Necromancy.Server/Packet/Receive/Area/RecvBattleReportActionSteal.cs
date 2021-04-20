using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionSteal : PacketResponse
    {
        public RecvBattleReportActionSteal()
            : base((ushort)AreaPacketId.recv_battle_report_action_steal, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            for (int i = 0; i < 0x10; i++) res.WriteByte(0); //Either 16 bytes, or a fixed string with size of 16
            return res;
        }
    }
}
