using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfSalvageEnd : PacketResponse
    {
        private readonly int _code;

        public RecvCharaBodySelfSalvageEnd(int code)
            : base((ushort)AreaPacketId.recv_charabody_self_salvage_end, ServerType.Area)
        {
            _code = code;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_code);
            return res;
        }
    }
}
