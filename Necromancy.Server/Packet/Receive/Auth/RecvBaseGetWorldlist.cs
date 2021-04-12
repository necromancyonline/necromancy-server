using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Auth
{
    public class RecvBaseGetWorldlist : PacketResponse
    {
        private readonly uint _numOfWorlds;

        public RecvBaseGetWorldlist(uint numOfWorlds)
            : base((ushort)AuthPacketId.recv_base_get_worldlist_r, ServerType.Auth)
        {
            _numOfWorlds = numOfWorlds;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_numOfWorlds);
            for (int i = 1; i <= _numOfWorlds; i++)
            {
                res.WriteInt32(i); // World ID
                res.WriteFixedString($"World {i}", 37);
                res.WriteInt32(0);
                res.WriteInt16(0); //Max Player
                res.WriteInt16(0); //Current Player
            }

            res.WriteByte(1); //cmp with worldID
            res.WriteByte(7);
            res.WriteByte(8);
            res.WriteByte(9);
            res.WriteByte(0); // 1 = Server merge notice
            return res;
        }
    }
}
