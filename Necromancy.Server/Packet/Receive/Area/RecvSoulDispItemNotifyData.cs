using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulDispItemNotifyData : PacketResponse
    {
        /// <summary>
        /// Adds Items to your 'Valulables' tab under Quest Mission.  
        /// </summary>
        private int _serialId;
        public RecvSoulDispItemNotifyData(int serialId)
            : base((ushort) AreaPacketId.recv_soul_dispitem_notify_data, ServerType.Area)
        {
            _serialId = serialId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_serialId);
            return res;
        }
    }
}
