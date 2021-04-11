using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateLv : PacketResponse
    {
        uint _instanceId;
        short _level;
        public RecvCharaUpdateLv(Character character)
            : base((ushort) AreaPacketId.recv_chara_update_lv, ServerType.Area)
        {
            _instanceId = character.instanceId;
            _level = character.level;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId); //objectid
            res.WriteInt16(_level); //level
            return res;
        }
    }
}
