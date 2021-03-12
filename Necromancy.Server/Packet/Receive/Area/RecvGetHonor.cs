using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGetHonor : PacketResponse
    {
        private int _honorId;
        private uint _characterInstanceId;
        private byte _newOrOld;
        public RecvGetHonor(int honorId, uint characterInstanceId, byte newOrOld)
            : base((ushort) AreaPacketId.recv_get_honor, ServerType.Area)
        {
            _honorId = honorId;
            _characterInstanceId = characterInstanceId;
            _newOrOld = newOrOld;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_honorId);
            res.WriteUInt32(_characterInstanceId);
            res.WriteByte(_newOrOld);//bool
            return res;
        }
    }
}
