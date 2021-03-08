using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyLootItem : PacketResponse
    {
        private byte _fromZone;
        private byte _fromContainer;
        private short _fromSlot;
        private short _itemCount;
        private string _soulName;
        private string _charaName;
        public RecvCharaBodyNotifyLootItem(byte fromZone, byte fromContainer, short fromSlot, short itemCount, string soulName, string charaName)
            : base((ushort)AreaPacketId.recv_charabody_notify_loot_item, ServerType.Area)
        {            
            _fromZone = fromZone;
            _fromContainer = fromContainer;
            _fromSlot = fromSlot;
            _itemCount = itemCount;
            _soulName = soulName;
            _charaName = charaName;            
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(_fromZone);
            res.WriteByte(_fromContainer);
            res.WriteInt16(_fromSlot);

            res.WriteInt16(_itemCount); //Number here is "pieces" 
            res.WriteCString(_soulName); // Length 0x31 
            res.WriteCString(_charaName); // Length 0x5B
            return res;
        }
    }
}
