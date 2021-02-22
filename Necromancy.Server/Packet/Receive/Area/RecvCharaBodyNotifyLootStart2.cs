using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyLootStart2 : PacketResponse
    {
        public RecvCharaBodyNotifyLootStart2()
            : base((ushort) AreaPacketId.recv_charabody_notify_loot_start2, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //res.WriteByte(fromZone);
            //res.WriteByte(fromContainer);
            //res.WriteInt16(fromSlot);
            //res.WriteFloat(10); //base loot time
            //res.WriteFloat(15); // loot time
            //res.WriteCString($"{client.Soul.Name}"); // soul name
            //res.WriteCString($"{client.Character.Name}"); // chara name
            return res;
        }
    }
}
