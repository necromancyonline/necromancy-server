using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUse : PacketResponse
    {
        private int _result;
        private float _cooltime;
        public RecvItemUse(int result, float cooltime)
            : base((ushort) AreaPacketId.recv_item_use_r, ServerType.Area)
        {
            _result = result;
            _cooltime = cooltime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_result);
            res.WriteFloat(_cooltime);
            return res;
        }
    }
}
