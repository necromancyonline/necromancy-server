using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyNpcExDragon : PacketResponse
    {
        private readonly uint _objectId;

        public RecvDataNotifyNpcExDragon(uint objectId)
            : base((ushort)AreaPacketId.recv_data_notify_npc_ex_dragon, ServerType.Area)
        {
            _objectId = objectId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_objectId);
            return res;
        }
    }
}
