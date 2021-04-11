using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvClassAdvancementNotifyLearnAll : PacketResponse
    {
        public RecvClassAdvancementNotifyLearnAll()
            : base((ushort) AreaPacketId.recv_class_advancement_notify_learn_all, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            return res;
        }
    }
}
