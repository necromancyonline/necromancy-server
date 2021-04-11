using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerStorageOpen : PacketResponse
    {
        public RecvSoulPartnerStorageOpen()
            : base((ushort) AreaPacketId.recv_soul_partner_storage_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(0);
            res.WriteInt32(numEntries); //less than 5
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
            res.WriteInt32(numEntries); //less than 0xC8
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
            res.WriteInt32(numEntries); //less than 0x32
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
            res.WriteInt32(numEntries); //less than 0x3
            //495fD0
            for (int k = 0; k < numEntries; k++)
            {
                res.WriteFixedString("Xeno", 0x61);
                for (int l = 0; l < 0x5; l++) //must be 5

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
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteByte(0);
            }//end 495FD0
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteByte(0);

            res.WriteInt32(numEntries); //less than 0x40
            //496190
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt16(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int k = 0; k < 0x7; k++)
                {
                    res.WriteInt16(0);
                }
                res.WriteInt32(0);
                res.WriteInt32(0);
                for (int k = 0; k < 0xA; k++)
                {
                    //4960C0
                    res.WriteInt64(0);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteFixedString("Shoot me", 0x10);
                }
                res.WriteInt16(0);
            }//end 496190

            res.WriteInt32(numEntries); //less than 0x7
            //4962B0
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt16(0);
            }
            return res;
        }
    }
}
