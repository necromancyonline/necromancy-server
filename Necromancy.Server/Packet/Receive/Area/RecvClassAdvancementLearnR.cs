using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvClassAdvancementLearnR : PacketResponse
    {
        public RecvClassAdvancementLearnR()
            : base((ushort) AreaPacketId.recv_class_advancement_learn_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //throws last chance if 0.

            return res;
        }
    }
}
