using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifyCrimeLv : PacketResponse
    {
        private uint _id;
        private byte _crimeLevel;
        public RecvCharaBodyNotifyCrimeLv(uint id, byte crimeLevel)
            : base((ushort) AreaPacketId.recv_charabody_notify_crime_lv, ServerType.Area)
        {
            _id = id;
            _crimeLevel = crimeLevel;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id);
            res.WriteByte(_crimeLevel);
            return res;
        }
    }
}
