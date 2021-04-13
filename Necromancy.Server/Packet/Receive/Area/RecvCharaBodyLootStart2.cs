using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyLootStart2 : PacketResponse
    {
        private readonly int _lootTime;
        private readonly int _result;

        public RecvCharaBodyLootStart2(int result, int lootTime)
            : base((ushort)AreaPacketId.recv_charabody_loot_start2_r, ServerType.Area)
        {
            _result = result;
            _lootTime = lootTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_result); //Result
            res.WriteInt32(_lootTime); //LootTime
            return res;
        }
    }
}
