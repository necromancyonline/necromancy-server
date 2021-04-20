using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvWantedUpdateStateActorNotify : PacketResponse
    {
        public RecvWantedUpdateStateActorNotify()
            : base((ushort)AreaPacketId.recv_wanted_update_state_actor_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(""); // Length 0x31
            res.WriteInt32(0);
            return res;
        }
    }
}
