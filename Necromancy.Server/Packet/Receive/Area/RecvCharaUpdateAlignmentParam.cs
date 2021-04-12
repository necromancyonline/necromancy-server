using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateAlignmentParam : PacketResponse
    {
        public RecvCharaUpdateAlignmentParam()
            : base((ushort)AreaPacketId.recv_chara_update_alignment_param, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Lawful
            res.WriteInt32(0); //Neutral
            res.WriteInt32(0); //Chaotic
            return res;
        }
    }
}
