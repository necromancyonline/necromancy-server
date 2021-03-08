using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfDragonWarpNotify : PacketResponse
    {
        private int _objectId;
        public RecvSelfDragonWarpNotify(int objectId)
            : base((ushort) AreaPacketId.recv_self_dragon_warp_notify, ServerType.Area)
        {
            _objectId = objectId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_objectId);
            return res;
        }
    }
}
