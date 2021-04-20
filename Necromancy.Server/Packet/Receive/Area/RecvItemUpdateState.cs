using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateState : PacketResponse
    {
        public RecvItemUpdateState()
            : base((ushort)AreaPacketId.recv_item_update_state, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0); //item instance id
            res.WriteInt32(0); //item state flag, same as inside SendItemInstance and SendItemInstanceUnidentified
            return res;
        }
    }
}
