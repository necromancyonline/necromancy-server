using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_challengemap_ranking_start : PacketResponse
    {
        public recv_challengemap_ranking_start()
            : base((ushort) AreaPacketId.recv_challengemap_ranking_start, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;

            res.WriteInt32(0);

            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0x31);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0x5B);
            for (int i = 0; i < 0x3; i++)
            {
                res.WriteInt64(0);
            }

            res.WriteInt32(numEntries);  //Less than 0x64
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("Xeno", 0x31);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("Xeno", 0x5B);
                for (int i = 0; i < 0x3; i++)
                {
                    res.WriteInt64(0);
                }
            }
            return res;
        }
    }
}
