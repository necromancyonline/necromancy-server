using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySalvageRequestCancel : PacketResponse
    {
        private readonly int _code;

        public RecvCharaBodySalvageRequestCancel(int code)
            : base((ushort)AreaPacketId.recv_charabody_salvage_request_cancel_r, ServerType.Area)
        {
            _code = code;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_code); //sends "corpse recovery failed"
            return res;
        }
    }
}
