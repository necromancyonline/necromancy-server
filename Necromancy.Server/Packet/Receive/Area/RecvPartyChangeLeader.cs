using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyChangeLeader : PacketResponse
    {
        private uint _charaInstanceId;
        public RecvPartyChangeLeader(uint charaInstanceId)
            : base((ushort) AreaPacketId.recv_party_change_leader_r, ServerType.Area)
        {
            _charaInstanceId = charaInstanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_charaInstanceId);
            return res;
        }
    }
}
