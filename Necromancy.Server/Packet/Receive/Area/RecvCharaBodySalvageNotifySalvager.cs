using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    //this is for when your body is collected.  it turns your soul invisible and imovable.  You should be 'possessing' your salavager when this is called.
    public class RecvCharaBodySalvageNotifySalvager : PacketResponse
    {
        private readonly string _charaName;
        private readonly uint _id;
        private readonly string _soulName;

        public RecvCharaBodySalvageNotifySalvager(uint id, string charaName, string soulName)
            : base((ushort)AreaPacketId.recv_charabody_salvage_notify_salvager, ServerType.Area)
        {
            _id = id;
            _charaName = charaName;
            _soulName = soulName;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id); //salvage_objectId
            res.WriteCString(_charaName); // Length 0x31
            res.WriteCString(_soulName); // Length 0x5B
            return res;
        }
    }
}
