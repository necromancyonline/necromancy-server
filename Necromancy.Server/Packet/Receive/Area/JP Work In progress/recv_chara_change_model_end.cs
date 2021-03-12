using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_chara_change_model_end : PacketResponse
    {
        public recv_chara_change_model_end()
            : base((ushort) AreaPacketId.recv_chara_change_model_end, ServerType.Area)
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
