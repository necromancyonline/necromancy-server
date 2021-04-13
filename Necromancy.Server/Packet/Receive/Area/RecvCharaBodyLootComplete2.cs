using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyLootComplete2 : PacketResponse
    {
        private readonly int _remainLootTime;
        private readonly int _result;

        public RecvCharaBodyLootComplete2(int result, int remainLootTime)
            : base((ushort)AreaPacketId.recv_charabody_loot_complete2_r, ServerType.Area)
        {
            _result = result;
            _remainLootTime = remainLootTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_result); //result
            res.WriteInt32(_remainLootTime); //remainLootTime
            return res;
        }
    }
}
