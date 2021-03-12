using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_status_open_r : PacketResponse
    {
        public recv_soul_partner_status_open_r()
            : base((ushort) AreaPacketId.recv_soul_partner_status_open_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(numEntries); //less than 0x5
            for (int k = 0; k < numEntries; k++)
            {
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
            }

            res.WriteInt64(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt64(0);
            return res;
        }
    }
}
