using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvHelpNewData : PacketResponse
    {
        public RecvHelpNewData()
            : base((ushort)AreaPacketId.recv_help_new_data, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //<=0xA
            return res;
        }
    }
}
