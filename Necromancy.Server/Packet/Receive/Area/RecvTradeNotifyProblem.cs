using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTradeNotifyProblem : PacketResponse
    {
        private uint _objectId;
        private byte _systemMessage;
        public RecvTradeNotifyProblem(uint objectId, byte systemMessage)
            : base((ushort) AreaPacketId.recv_trade_notify_problem, ServerType.Area)
        {
            _objectId = objectId;
            _systemMessage = systemMessage;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_objectId); //optional instanceId for system messages
            res.WriteByte(_systemMessage); //which system message to throw
            return res;
        }
    }
}
