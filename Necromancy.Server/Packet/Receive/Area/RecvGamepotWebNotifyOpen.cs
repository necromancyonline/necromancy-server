using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGamepotWebNotifyOpen : PacketResponse
    {
        public RecvGamepotWebNotifyOpen()
            : base((ushort)AreaPacketId.recv_gamepot_web_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(""); //find max size
            return res;
        }
    }
}
