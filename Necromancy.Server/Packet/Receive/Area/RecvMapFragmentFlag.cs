using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvMapFragmentFlag : PacketResponse
    {
        private readonly int _flag;
        private readonly int _id;

        public RecvMapFragmentFlag(int id, int flag)
            : base((ushort)AreaPacketId.recv_map_fragment_flag, ServerType.Area)
        {
            _id = id;
            _flag = flag;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_id); //MapSerialID
            res.WriteInt32(_flag); // Fragment bitmap
            return res;
        }
    }
}
