using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfSalvageNotify : PacketResponse
    {
        private readonly string _charaName;
        private readonly string _soulName;

        public RecvCharaBodySelfSalvageNotify(string charaName, string soulName)
            : base((ushort)AreaPacketId.recv_charabody_self_salvage_notify, ServerType.Area)
        {
            _charaName = charaName;
            _soulName = soulName;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(_charaName); //Length is 31-02=2F=DEC47
            res.WriteCString(_soulName); //Length is 5b-01=5A=Dec132
            return res;
        }
    }
}
