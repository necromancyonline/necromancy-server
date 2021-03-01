using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySalvageNotifyBody : PacketResponse
    {
        private uint _id;
        private string _charaName;
        private string _soulName;
        public RecvCharaBodySalvageNotifyBody(uint id, string charaName, string soulName)
            : base((ushort) AreaPacketId.recv_charabody_salvage_notify_body, ServerType.Area)
        {
            _id = id;
            _charaName = charaName;
            _soulName = soulName;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id); //body_objectId
            res.WriteCString(_charaName); // Length 0x31
            res.WriteCString(_soulName); // Length 0x5B
            return res;
        }
    }
}
