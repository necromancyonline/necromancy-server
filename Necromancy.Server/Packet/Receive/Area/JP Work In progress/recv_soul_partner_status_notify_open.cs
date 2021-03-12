using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_status_notify_open : PacketResponse
    {
        public recv_soul_partner_status_notify_open()
            : base((ushort) AreaPacketId.recv_soul_partner_status_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0x2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(numEntries); //less than 0x5 
            for (int j = 0; j < numEntries; j++)
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
                    for (int k = 0; k < 0x7; k++)
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
            }

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
