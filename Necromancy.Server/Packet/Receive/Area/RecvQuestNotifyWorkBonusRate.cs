using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestNotifyWorkBonusRate : PacketResponse
    {
        public RecvQuestNotifyWorkBonusRate()
            : base((ushort)AreaPacketId.recv_quest_notify_work_bonus_rate, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(numEntries); //less than 0x1E
            for (int k = 0; k < numEntries; k++)
            {
                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
            }

            res.WriteInt32(numEntries); //less than 0x14
            for (int k = 0; k < numEntries; k++)
            {
                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
            }

            return res;
        }
    }
}
