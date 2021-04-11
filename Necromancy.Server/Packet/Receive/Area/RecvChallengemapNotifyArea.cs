using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapNotifyArea : PacketResponse
    {
        public RecvChallengemapNotifyArea()
            : base((ushort) AreaPacketId.recv_challengemap_notify_area, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(numEntries); //less than 8
            for (int k = 0; k < numEntries; k++)
            {
                //sub 495A00
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt16(0);

                res.WriteByte(0);

                res.WriteFixedString("Test", 0x61);
                res.WriteFixedString("Test2", 0x61);
                res.WriteFixedString("Test3", 0x181);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt16(0);
                res.WriteInt64(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);

                res.WriteInt32(0);

                res.WriteInt32(0);
                //sub_495940
                res.WriteInt32(0);
                res.WriteFixedString("Test4", 0x31);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("Test4", 0x5B);
                for (int i = 0; i < 0x3; i++)
                {
                    res.WriteInt64(0);
                }

                for (int j = 0; j < 0x64; j++)
                {
                    //sub_495940
                    res.WriteInt32(0);
                    res.WriteFixedString("Test4", 0x31);
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    res.WriteFixedString("Test4", 0x5B);
                    for (int i = 0; i < 0x3; i++)
                    {
                        res.WriteInt64(0);
                    }
                }
            }
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
