using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapAreaRankingCloseR : PacketResponse
    {
        public RecvChallengemapAreaRankingCloseR()
            : base((ushort) AreaPacketId.recv_challengemap_area_ranking_close_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            //4th miss.  check xdbg
            return res;
        }
    }
}
