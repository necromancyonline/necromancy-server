using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvWantedJailUpdateMoney : PacketResponse
    {
        public RecvWantedJailUpdateMoney()
            : base((ushort) AreaPacketId.recv_wanted_jail_update_money, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(10000000);
            return res;
        }
    }
}
