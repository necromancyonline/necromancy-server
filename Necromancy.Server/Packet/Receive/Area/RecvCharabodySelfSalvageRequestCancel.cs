using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharabodySelfSalvageRequestCancel : PacketResponse
    {
        private readonly uint _id;

        public RecvCharabodySelfSalvageRequestCancel(uint id)
            : base((ushort)AreaPacketId.recv_charabody_self_salvage_request_cancel, ServerType.Area)
        {
            _id = id;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id);
            return res;
        }
    }
}
