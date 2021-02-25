using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySalvageEnd : PacketResponse
    {
        private int _code;
        private uint _id;
        public RecvCharaBodySalvageEnd(int code, uint id)
            : base((ushort) AreaPacketId.recv_charabody_salvage_end, ServerType.Area)
        {
            _code = code;
            _id = id;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_code); //reason
            res.WriteUInt32(_id); //objectId
            return res;
        }
    }
}
