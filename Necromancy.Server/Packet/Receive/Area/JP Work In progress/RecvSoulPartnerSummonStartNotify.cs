using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_summon_start_notify : PacketResponse
    {
        public recv_soul_partner_summon_start_notify()
            : base((ushort) AreaPacketId.recv_soul_partner_summon_start_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //  Dude you forgot to write down the structure.  ...... go find it again in xdbg.....
            res.WriteInt32(200000002);  //chara instance id
            res.WriteInt32(100001);  //object id
            res.WriteInt64(100001);  //serial id
            res.WriteInt32(100001);  //base id
            res.WriteCString("Failboat"); 
            res.WriteByte(1); //Slot
            res.WriteInt32(5); // SG 
            res.WriteByte(1); //is awakening
            res.WriteByte(31); //Avatar ID
            return res;
        }
    }
}
