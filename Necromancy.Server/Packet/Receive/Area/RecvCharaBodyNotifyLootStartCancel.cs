using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyLootStartCancel : PacketResponse
    {
        private byte _fromZone;
        private byte _fromContainer;
        private short _fromSlot;
        private string _soulName;
        private string _charaName;
        public RecvCharaBodyNotifyLootStartCancel(byte fromZone, byte fromContainer, short fromSlot, string soulName, string charaName)
            : base((ushort) AreaPacketId.recv_charabody_notify_loot_start_cancel, ServerType.Area)
        {
            _fromZone = fromZone;
            _fromContainer = fromContainer;
            _fromSlot = fromSlot;
            _soulName = soulName;
            _charaName = charaName;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(_fromZone);
            res.WriteByte(_fromContainer);
            res.WriteInt16(_fromSlot);
            res.WriteCString(_soulName);//Length 31-2=DEC47
            res.WriteCString(_charaName);//Length 5B-1=DEC90
            return res;
        }
    }
}
