using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaNotifyPartyJoin : PacketResponse
    {
        private readonly uint _charaInstanceId;
        private readonly uint _partyInstanceId;
        private readonly int _partyMode;

        public RecvCharaNotifyPartyJoin(uint charaInstanceId, uint partyInstanceId, int partyMode)
            : base((ushort)AreaPacketId.recv_chara_notify_party_join, ServerType.Area)
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
