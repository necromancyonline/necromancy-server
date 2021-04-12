using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSkillTreeGain : PacketResponse
    {
        public RecvSkillTreeGain()
            : base((ushort)AreaPacketId.recv_skill_tree_gain, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0); //Level?
            res.WriteByte(0); //Bool
            res.WriteByte(0); //Bool
            return res;
        }
    }
}
