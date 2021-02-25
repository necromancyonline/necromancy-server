using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySelfNotifyDeadNextTime : PacketResponse
    {
        int _deadNextTime;
        public RecvCharaBodySelfNotifyDeadNextTime(int deadNextTime)
            : base((ushort) AreaPacketId.recv_charabody_self_notify_deadnext_time, ServerType.Area)
        {
            _deadNextTime = deadNextTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_deadNextTime); // time dead length. (time until you reach the next dead state.  faint-->Dead-->ash-->lost
            return res;
        }
    }
}
