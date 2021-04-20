using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyNotifyGetItem : PacketResponse
    {
        private readonly uint _charaInstanceId; //???

        public RecvPartyNotifyGetItem(uint charaInstanceId)
            : base((ushort)AreaPacketId.recv_party_notify_get_item, ServerType.Area)
        {
            _charaInstanceId = charaInstanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_charaInstanceId);
            res.WriteCString(" Area really long item name");
            res.WriteByte(55);
            return res;
        }
    }
}
