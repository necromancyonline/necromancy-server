using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvWantedListOpen : PacketResponse
    {
        public RecvWantedListOpen()
            : base((ushort)AreaPacketId.recv_wanted_list_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
