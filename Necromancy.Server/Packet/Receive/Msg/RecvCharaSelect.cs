using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCharaSelect : PacketResponse
    {
        public RecvCharaSelect()
            : base((ushort)MsgPacketId.recv_chara_select_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //possibly fake
            res.WriteInt32(0); //possibly fake
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0); //Bool
            res.WriteFixedString("", 0x41); //Fixed string of size 0x41
            res.WriteInt16(0);

            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteByte(0);

            return res;
        }
    }
}
