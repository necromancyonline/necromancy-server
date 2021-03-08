using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfSalvageResult : PacketResponse
    {
        private int _code;
        public RecvCharaBodySelfSalvageResult(int code)
            : base((ushort) AreaPacketId.recv_charabody_self_salvage_result, ServerType.Area)
        {
            _code = code;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_code);//result
            return res;
        }
    }
}
