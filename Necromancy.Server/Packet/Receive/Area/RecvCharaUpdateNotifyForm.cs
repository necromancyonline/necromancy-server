using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateNotifyForm : PacketResponse
    {
        public RecvCharaUpdateNotifyForm()
            : base((ushort) AreaPacketId.recv_chara_update_notify_form, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
