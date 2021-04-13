using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvTest : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly uint _targetInstanceId;

        public RecvTest(uint instanceId, uint targetInstanceId)
            : base((ushort)AreaPacketId.recv_0x1489, ServerType.Area)
        {
            _instanceId = instanceId;
            _targetInstanceId = targetInstanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No String - push wizardryonline_patched.4D6044
            res.WriteUInt32(_instanceId);
            res.WriteUInt32(_targetInstanceId);

            return res;
        }
    }
}
