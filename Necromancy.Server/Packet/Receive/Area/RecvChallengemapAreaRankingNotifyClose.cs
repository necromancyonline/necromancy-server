using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapAreaRankingNotifyClose : PacketResponse
    {
        public RecvChallengemapAreaRankingNotifyClose()
            : base((ushort) AreaPacketId.recv_challengemap_area_ranking_notify_close, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //no structure

            return res;
        }
    }
}
