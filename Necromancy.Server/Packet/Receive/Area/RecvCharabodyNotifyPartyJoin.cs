using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyPartyJoin : PacketResponse
    {
        private uint _charaInstanceId;
        private uint _partyInstanceId;
        private int _partyMode;
        public RecvCharaBodyNotifyPartyJoin(uint charaInstanceId, uint partyInstanceId, int partyMode)
            : base((ushort) AreaPacketId.recv_charabody_notify_party_join, ServerType.Area)
        {
            _charaInstanceId = charaInstanceId;
            _partyInstanceId = partyInstanceId;
            _partyMode = partyMode;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_charaInstanceId);
            res.WriteUInt32(_partyInstanceId);
            res.WriteInt32(_partyMode);
            return res;
        }
    }
}
