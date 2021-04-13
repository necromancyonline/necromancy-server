using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvJobChangeNotifyExe : PacketResponse
    {
        public RecvJobChangeNotifyExe()
            : base((ushort)AreaPacketId.recv_job_change_notify_exe, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            int numEntries = 19;
            res.WriteInt32(numEntries); // less than or equal to 19
            for (int i = 0; i < numEntries; i++) res.WriteInt32(0);
            numEntries = 19;
            res.WriteInt32(numEntries); //less than or equal to 19
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

            numEntries = 19;
            res.WriteInt32(numEntries); //less than or equal to 19
            for (int i = 0; i < numEntries; i++) res.WriteInt32(0);
            return res;
        }
    }
}
