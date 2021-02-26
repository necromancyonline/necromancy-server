using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySalvageEnd : PacketResponse
    {
        private int _code;
        private uint _id;
        public RecvCharaBodySalvageEnd(int code, uint id)
            : base((ushort) AreaPacketId.recv_charabody_salvage_end, ServerType.Area)
        {
            _code = code;
            _id = id;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_code); //reason
            res.WriteUInt32(_id); //objectId
            return res;
        }

        //        0	The corpse fell on the spot as it returned to the soul.
        //1	The corpse was abandoned in %s.
        //2	%1%s carried the corpse of %2%s to the temple.
        //3	As %s logged out"," the corpse fell into place and returned to the soul.
        //4	
        //5	The corpse fell on the spot because the %s line was disconnected.

    }
}
