using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGemSpallInfo : PacketResponse
    {
        public RecvGemSpallInfo()
            : base((ushort) AreaPacketId.recv_gem_spall_info, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //sub 496470
            for (int j = 0; j < 0x3; j++)
            {
                for (int k = 0; k < 0x2; k++)
                {
                    res.WriteInt32(0);
                    res.WriteFixedString("Xeno", 0x10);
                }
                res.WriteInt16(0);
            }
            res.WriteInt16(0);
            res.WriteInt64(0);
            //end sub
            return res;
        }
    }
}
