using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_roguemap_entry_notify_close : PacketResponse
    {
        public recv_roguemap_entry_notify_close()
            : base((ushort) AreaPacketId.recv_roguemap_entry_notify_close, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No Structure	

            return res;
        }
    }
}
