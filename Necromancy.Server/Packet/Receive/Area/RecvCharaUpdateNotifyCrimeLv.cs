using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateNotifyCrimeLv : PacketResponse
    {
        public RecvCharaUpdateNotifyCrimeLv()
            : base((ushort)AreaPacketId.recv_chara_update_notify_crime_lv, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteByte(0);
            return res;
        }
    }
}
