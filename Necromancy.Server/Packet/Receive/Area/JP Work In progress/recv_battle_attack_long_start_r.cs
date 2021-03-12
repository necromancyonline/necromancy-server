using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_battle_attack_long_start_r : PacketResponse
    {
        public recv_battle_attack_long_start_r()
            : base((ushort) AreaPacketId.recv_battle_attack_long_start_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(0);
            res.WriteInt32(numEntries); //less than 0x4 
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteFloat(0);
            }
            return res;
        }
    }
}
