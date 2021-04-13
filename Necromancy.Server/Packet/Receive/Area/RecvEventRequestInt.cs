using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventRequestInt : PacketResponse
    {
        private readonly string _displayText;
        private readonly int _initialAmount;
        private readonly int _maxAmount;
        private readonly int _minAmount;

        public RecvEventRequestInt(string displayText, int minAmount, int maxAmount, int initialAmount)
            : base((ushort)AreaPacketId.recv_event_request_int, ServerType.Area)
        {
            _displayText = displayText;
            _minAmount = minAmount;
            _maxAmount = maxAmount;
            _initialAmount = initialAmount;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(_displayText);
            res.WriteInt32(_minAmount);
            res.WriteInt32(_maxAmount);
            res.WriteInt32(_initialAmount);

            return res;
        }
    }
}
