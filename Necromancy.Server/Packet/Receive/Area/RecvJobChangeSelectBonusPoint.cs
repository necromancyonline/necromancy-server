using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvJobChangeSelectBonusPoint : PacketResponse
    {
        public RecvJobChangeSelectBonusPoint()
            : base((ushort)AreaPacketId.recv_job_change_select_bonuspoint_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            int numEntries = 0x7;
            for (int i = 0; i < numEntries; i++) res.WriteInt16(0);

            int numEntries2 = 0x9;
            for (int i = 0; i < numEntries2; i++) res.WriteInt16(0);

            int numEntries3 = 0x9;
            for (int i = 0; i < numEntries3; i++) res.WriteInt16(0);

            int numEntries4 = 0xB;
            for (int i = 0; i < numEntries4; i++) res.WriteInt16(0);


            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
