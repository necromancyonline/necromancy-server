using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCharaGetCreateInfo : PacketResponse
    {
        public RecvCharaGetCreateInfo()
            : base((ushort)MsgPacketId.recv_chara_get_createinfo_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(8);
            for (int i = 0; i < 8; i++) //(int32 above)
                res.WriteByte(0);
            res.WriteInt32(8);
            for (int i = 0; i < 8; i++) //(int32 above)
                res.WriteByte(0);
            res.WriteInt32(8);
            for (int i = 0; i < 8; i++) //(int32 above)
                res.WriteByte(0);
            res.WriteInt32(5);
            for (int i = 0; i < 5; i++) //(int32 above)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 7; j++)
                    res.WriteInt16(0);
            }

            res.WriteInt32(10);
            for (int i = 0; i < 10; i++) //(int32 above)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                for (int j = 0; j < 19; j++)
                    res.WriteInt32(0);
                for (int k = 0; k < 19; k++)
                {
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0); //bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                }

                for (int l = 0; l < 19; l++)
                    res.WriteInt32(0);
                res.WriteByte(0);
            }

            res.WriteInt32(0x8C); //0x8C is too much for the packet size
            for (int i = 0; i < 0x8C; i++) //(int32 above)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 19; j++)
                    res.WriteInt32(0);
                for (int k = 0; k < 19; k++)
                {
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0); //bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                }

                for (int l = 0; l < 19; l++)
                    res.WriteInt32(0);
                res.WriteByte(0);
            }

            res.WriteInt32(14);
            for (int i = 0; i < 14; i++) //(int32 above)
            {
                res.WriteInt32(0);
                res.WriteByte(0);
                for (int j = 0; j < 7; j++)
                    res.WriteInt16(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 7; j++)
                    res.WriteInt16(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
            }

            return res;
        }
    }
}
