using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfWarpDragonPenalty : PacketResponse
    {
        private readonly uint _time;

        public RecvCharaBodySelfWarpDragonPenalty(uint time)
            : base((ushort)AreaPacketId.recv_charabody_self_warpdragon_penalty, ServerType.Area)
        {
            _time = time;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_time); //time
            return res;
        }
    }
}
