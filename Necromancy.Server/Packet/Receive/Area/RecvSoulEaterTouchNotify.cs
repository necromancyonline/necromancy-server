using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    /// <summary>
    ///     when the walkers get you, fire this.
    /// </summary>
    public class RecvSoulEaterTouchNotify : PacketResponse
    {
        public RecvSoulEaterTouchNotify()
            : base((ushort)AreaPacketId.recv_souleater_touch_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //objectId
            res.WriteInt32(0); //is_success
            return res;
        }
    }
}
