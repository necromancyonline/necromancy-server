using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTempleNotifyOpen : PacketResponse
    {
        /// <summary>
        /// Opens a shop.  2 is the remove curse tab
        /// </summary>
        private byte _unknown;
        public RecvTempleNotifyOpen(byte unknown)
            : base((ushort) AreaPacketId.recv_temple_notify_open, ServerType.Area)
        {
            _unknown = unknown;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(_unknown);
            return res;
        }
    }
}
