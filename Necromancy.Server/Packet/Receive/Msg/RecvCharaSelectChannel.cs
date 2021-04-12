using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCharaSelectChannel : PacketResponse
    {
        public RecvCharaSelectChannel()
            : base((ushort)MsgPacketId.recv_chara_select_channel_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0);
            res.WriteByte(0);

            for (int i = 0; i < 5; i++)
            {
                res.WriteByte(0); //bool
                res.WriteByte(0);
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0x80; j++)
                {
                    res.WriteInt32(0);
                    res.WriteFixedString("", 0x61);
                    res.WriteByte(0); //bool
                    res.WriteInt16(0);
                    res.WriteInt16(0);
                    res.WriteByte(0);
                }

                res.WriteByte(0);
            }

            res.WriteByte(0);
            return res;
        }
    }
}
