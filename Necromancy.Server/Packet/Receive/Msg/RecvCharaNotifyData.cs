using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCharaNotifyData : PacketResponse
    {
        public RecvCharaNotifyData()
            : base((ushort) MsgPacketId.recv_chara_notify_data, ServerType.Msg)
        {
        }
        //ToDo,  figure out why this is in Msg.  could be a big deal actually
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteFixedString("", 0x5B);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            for (int i = 0; i < 0x19; i++) //item stuff
                res.WriteInt32(0);

            for (int i = 0; i < 0x19; i++) //Item stuff
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
                res.WriteByte(0);
            }

            for (int i = 0; i < 0x19; i++) //item stuff
                res.WriteInt32(0);

            for (int i = 0; i < 0x19; i++) //item stuff
                res.WriteInt32(0);

            for (int i = 0; i < 0x19; i++) //
                res.WriteByte(0);

            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteFixedString("", 0x5B);
            return res;
        }
    }
}
