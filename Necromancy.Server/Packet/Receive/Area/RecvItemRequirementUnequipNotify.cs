using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemRequirementUnequipNotify : PacketResponse
    {
        public RecvItemRequirementUnequipNotify()
            : base((ushort) AreaPacketId.recv_item_requirement_unequip_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
