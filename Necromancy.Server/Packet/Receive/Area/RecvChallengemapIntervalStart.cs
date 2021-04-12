using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapIntervalStart : PacketResponse
    {
        public RecvChallengemapIntervalStart()
            : base((ushort)AreaPacketId.recv_challengemap_interval_start, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //sub_4957C0
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            for (int k = 0; k < 0x3; k++) res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0xC1);
            res.WriteInt32(0);
            for (int k = 0; k < 0x3; k++) res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0xC1);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            //end sub
            return res;
        }
    }
}
