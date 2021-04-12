using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyUpdateDragon : PacketResponse
    {
        private readonly uint _id;

        public RecvPartyNotifyUpdateDragon(uint id)
            : base((ushort)MsgPacketId.recv_party_notify_update_dragon, ServerType.Msg)
        {
            _id = id;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id);
            res.WriteByte(1);
            return res;
        }
    }
}
