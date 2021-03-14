using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfMoneyNotify : PacketResponse
    {
        private readonly ulong _currentGold;
        public RecvSelfMoneyNotify(NecClient client, ulong currentGold)
            : base((ushort) AreaPacketId.recv_self_money_notify, ServerType.Area)
        {
            _currentGold = currentGold;
            Clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_currentGold);
            return res;
        }
    }
}
