using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfBuffNotify : PacketResponse
    {
        private readonly Buff[] _buffArr;

        public RecvSelfBuffNotify(Buff[] buffArr)
            : base((ushort)AreaPacketId.recv_self_buff_notify, ServerType.Area)
        {
            _buffArr = buffArr;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //            res.WriteInt32(_buffId);
            int numEntries = _buffArr.Length;
//            int numEmptyBuffs = numEntries - _buffArr.Length;
            res.WriteInt32(numEntries); //less than or equal to 0x80
            for (int i = 0; i < _buffArr.Length; i++)
            {
                res.WriteInt32(_buffArr[i].buffId);
                res.WriteInt32(_buffArr[i].unknown1);
                res.WriteInt32(_buffArr[i].unknown2);
            }
/*            for (int i = 0; i < numEmptyBuffs; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }   */

            return res;
        }
    }
}
