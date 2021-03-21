using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvForgeCheck : PacketResponse
    {
        public RecvForgeCheck()
            : base((ushort) AreaPacketId.recv_forge_check_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteByte(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
