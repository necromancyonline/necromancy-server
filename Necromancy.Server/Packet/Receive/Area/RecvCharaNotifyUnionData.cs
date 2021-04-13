using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaNotifyUnionData : PacketResponse
    {
        public RecvCharaNotifyUnionData()
            : base((ushort)AreaPacketId.recv_chara_notify_union_data, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteCString("ToBeFound");
            return res;
        }
    }
}
