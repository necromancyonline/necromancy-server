using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyPartyLeave : PacketResponse
    {
        private readonly uint _id;

        public RecvCharaBodyNotifyPartyLeave(uint id)
            : base((ushort)AreaPacketId.recv_charabody_notify_party_leave, ServerType.Area)
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
