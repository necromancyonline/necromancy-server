using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemScrollExportCheckR : PacketResponse
    {
        public RecvItemScrollExportCheckR()
            : base((ushort) AreaPacketId.recv_item_scroll_export_check_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0x10);
            res.WriteInt64(0);

            for (int j = 0; j < 0x5; j++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("Xeno", 0x10);
                res.WriteInt64(0);
            }
            return res;
        }
    }
}
