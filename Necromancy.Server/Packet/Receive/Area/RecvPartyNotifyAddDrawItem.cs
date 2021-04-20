using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyNotifyAddDrawItem : PacketResponse
    {
        private readonly int _iconId;
        private readonly ulong _instanceId;
        private readonly float _timeout;

        public RecvPartyNotifyAddDrawItem(ulong instanceId, float timeout, int iconId)
            : base((ushort)AreaPacketId.recv_party_notify_add_draw_item, ServerType.Area)
        {
            _instanceId = instanceId;
            _timeout = timeout;
            _iconId = iconId;
        }

        // Item instance must be sent first
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_instanceId); // InstanceId
            res.WriteFloat(_timeout); // Timer in seconds
            res.WriteInt32(_iconId); // A 0 here causes Join/Pass buttons

            return res;
        }
    }
}
