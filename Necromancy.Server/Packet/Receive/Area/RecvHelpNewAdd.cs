using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvHelpNewAdd : PacketResponse
    {
        public RecvHelpNewAdd()
            : base((ushort)AreaPacketId.recv_help_new_add, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteCString(""); //find max size
            res.WriteCString(""); //find max size
            return res;
        }
    }
}
