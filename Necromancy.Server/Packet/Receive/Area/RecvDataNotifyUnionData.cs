using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyUnionData : PacketResponse
    {
        private readonly Character _character;
        private readonly string _unionName;

        public RecvDataNotifyUnionData(Character character, string unionName)
            : base((ushort) AreaPacketId.recv_chara_notify_union_data, ServerType.Area)
        {
            _character = character;
            _unionName = unionName;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_character.instanceId);
            res.WriteInt32(_character.unionId);
            res.WriteCString(_unionName);
            return res;
        }
    }
}
