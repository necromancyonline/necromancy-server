using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvJobChangeParam : PacketResponse
    {
        public RecvJobChangeParam()
            : base((ushort)AreaPacketId.recv_job_change_param, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            for (int i = 0; i < 7; i++) res.WriteInt16(0);
            for (int i = 0; i < 9; i++) res.WriteInt16(0);
            for (int i = 0; i < 9; i++) res.WriteInt16(0);
            for (int i = 0; i < 0xB; i++) res.WriteInt16(0);

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);

            for (int i = 0; i < 7; i++) res.WriteInt16(0);
            for (int i = 0; i < 9; i++) res.WriteInt16(0);
            for (int i = 0; i < 9; i++) res.WriteInt16(0);
            for (int i = 0; i < 0xB; i++) res.WriteInt16(0);
            res.WriteInt64(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            int numEntries = 0x100;
            res.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++) res.WriteInt32(0);

            numEntries = 0x100;
            res.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++) res.WriteInt32(0);

            numEntries = 0x13;
            res.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++) res.WriteInt32(0);

            numEntries = 0x13;
            res.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++)
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

            numEntries = 0x13;
            res.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++) res.WriteInt32(0);
            return res;
        }
    }
}
