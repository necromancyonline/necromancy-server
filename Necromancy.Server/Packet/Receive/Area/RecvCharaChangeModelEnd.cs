using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaChangeModelEnd : PacketResponse
    {
        public RecvCharaChangeModelEnd()
            : base((ushort)AreaPacketId.recv_chara_change_model_end, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No structure
            return res;
        }
    }
}
