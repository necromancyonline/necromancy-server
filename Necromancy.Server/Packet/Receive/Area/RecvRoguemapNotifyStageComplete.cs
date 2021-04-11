using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRoguemapNotifyStageComplete : PacketResponse
    {
        public RecvRoguemapNotifyStageComplete()
            : base((ushort) AreaPacketId.recv_roguemap_notify_stage_complete, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;

            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(numEntries);//less than 0xE
            for (int j = 0; j < numEntries; j++)
            {
                //sub_496710
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                for (int i = 0; i < 0x4; i++)
                {
                    res.WriteInt32(0);
                }
                for (int i = 0; i < 0x4; i++)
                {
                    res.WriteInt32(0);
                }
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }
            return res;
        }
    }
}
