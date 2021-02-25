using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodyNotifySpirit : PacketResponse
    {
        private uint _id;
        private byte _valid;
        public RecvCharaBodyNotifySpirit(uint id, byte valid)
            : base((ushort) AreaPacketId.recv_charabody_notify_spirit, ServerType.Area)
        {
            _id = id;
            _valid = valid;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id); //object id
            res.WriteByte(_valid); //valid spirit
            return res;
        }
    }
}
