using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvThreadEntryMessage : PacketResponse
    {
        public RecvThreadEntryMessage()
            : base((ushort)AreaPacketId.recv_thread_entry_message, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(""); // Length 0x5B
            res.WriteCString(""); // Length 0x3D
            res.WriteInt16(0);
            return res;
        }
    }
}
