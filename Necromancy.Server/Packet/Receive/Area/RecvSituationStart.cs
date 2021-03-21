using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSituationStart : PacketResponse
    {
        private int _type;
        public RecvSituationStart(int type)
            : base((ushort) AreaPacketId.recv_situation_start, ServerType.Area)
        {
            _type = type;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_type); // 0 no output 1 chat only 2 chat and popup
            return res;
        }
    }
}
