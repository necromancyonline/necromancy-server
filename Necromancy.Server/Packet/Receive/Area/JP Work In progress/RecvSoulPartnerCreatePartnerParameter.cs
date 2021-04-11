using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_create_partner_parameter : PacketResponse
    {
        public recv_soul_partner_create_partner_parameter()
            : base((ushort) AreaPacketId.recv_soul_partner_create_partner_parameter, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //sub_495C70
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteFixedString("Xeno", 0x61);
            res.WriteFixedString("Xeno", 0x5B);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(0);

            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);

            for (int i = 0; i < 0x3; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int j = 0; j < 0x7; j++)
                {
                    res.WriteInt16(0);
                }
            }
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteByte(0);

            for (int i = 0; i < 0x3; i++)
            {
                res.WriteByte(0);
            }

            for (int i = 0; i < 0x5; i++)
            {
                res.WriteInt32(0);
            }

            for (int i = 0; i < 0x5; i++)
            {
                res.WriteByte(0);
            }
            //-----endsub
            res.WriteByte(0);
            return res;
        }
    }
}
