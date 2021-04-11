using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_fragment_request_skill_list_r : PacketResponse
    {
        public recv_soul_fragment_request_skill_list_r()
            : base((ushort) AreaPacketId.recv_soul_fragment_request_skill_list_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(numEntries); //less than 0x5
            for (int k = 0; k < numEntries; k++)
            {
                //sub_496E30
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
            }
            return res;
        }
    }
}
