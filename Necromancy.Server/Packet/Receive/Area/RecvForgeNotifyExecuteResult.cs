using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvForgeNotifyExecuteResult : PacketResponse
    {
        private uint _id;
        private int _result;
        public RecvForgeNotifyExecuteResult(uint id, int result)
            : base((ushort) AreaPacketId.recv_forge_notify_execute_result, ServerType.Area)
        {
            _id = id;
            _result = result;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_id);
            res.WriteInt32(_result);
            return res;
        }
    }
}
