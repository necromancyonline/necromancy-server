using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvClassAdvancementNotifySkillParam : PacketResponse
    {
        public RecvClassAdvancementNotifySkillParam()
            : base((ushort)AreaPacketId.recv_class_advancement_notify_skill_param, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 02;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(numEntries); //less than 0x40
            //4962B0
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
            }

            return res;
        }
    }
}
