using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySalvageRequest : PacketResponse
    {
        private int _code;
        public RecvCharaBodySalvageRequest(int code)
            : base((ushort) AreaPacketId.recv_charabody_salvage_request_r, ServerType.Area)
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
