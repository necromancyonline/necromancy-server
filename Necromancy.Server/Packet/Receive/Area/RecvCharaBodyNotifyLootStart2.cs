using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyLootStart2 : PacketResponse
    {
        private readonly float _baseLootTime;
        private readonly string _charaName;
        private readonly byte _fromContainer;
        private readonly short _fromSlot;
        private readonly byte _fromZone;
        private readonly float _lootTime;
        private readonly string _soulName;

        public RecvCharaBodyNotifyLootStart2(byte fromZone, byte fromContainer, short fromSlot, float baseLootTime, float lootTime, string soulName, string charaName)
            : base((ushort)AreaPacketId.recv_charabody_notify_loot_start2, ServerType.Area)
        {
            _fromZone = fromZone;
            _fromContainer = fromContainer;
            _fromSlot = fromSlot;
            _baseLootTime = baseLootTime;
            _lootTime = lootTime;
            _soulName = soulName;
            _charaName = charaName;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(_fromZone);
            res.WriteByte(_fromContainer);
            res.WriteInt16(_fromSlot);
            res.WriteFloat(_baseLootTime); //base loot time
            res.WriteFloat(_lootTime); // loot time
            res.WriteCString(_soulName); // soul name
            res.WriteCString(_charaName); // chara name
            return res;
        }
    }
}
