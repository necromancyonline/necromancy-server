using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaBodySalvageRequest : PacketResponse
    { 
    public enum ItemExceptionType
    {
            Cancelled = 0,
            Discarded = 1,
            Carried = 2,
            LoggedOut = 3,
            Lost = 4,
            Disconnected = 5,
            SelfCanceled = 10,
            Protected10 = -510,
            Protected13 = -513,
            Protected14 = -514,
            Protected28 = -528,
            PartySteal = -526,
            SoulRevive = -519,
            CorpseFull = -507,
        }
        private int _code;
        public RecvCharaBodySalvageRequest(int code)
            : base((ushort) AreaPacketId.recv_charabody_salvage_request_r, ServerType.Area)
        {
            _code = code;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_code);
            return res;
        }

        /// <summary>
        /// SALVAGE_DEADBODY	GENERIC	Failed to collect the corpse.
        //SALVAGE_DEADBODY	0	%s canceled the corpse collection"," so I dropped the collected corpse.
        //SALVAGE_DEADBODY    1	Discarded the corpse of %s.
        //SALVAGE_DEADBODY    2	Carried the corpse of %s to the temple.
        //SALVAGE_DEADBODY    3	The recovered corpse was dropped because%s logged out.
        //SALVAGE_DEADBODY    4	The soul of %s seems to have been lost ...
        //SALVAGE_DEADBODY    5	The recovered corpse was dropped because the %s line was disconnected.
        //SALVAGE_DEADBODY    10	Canceled the collection of %s corpse.
        //SALVAGE_DEADBODY    -510	It is protected by a mysterious power.
        //SALVAGE_DEADBODY    -513	It is protected by a mysterious power.
        //SALVAGE_DEADBODY    -514	It is protected by a mysterious power.
        //SALVAGE_DEADBODY    -528	It is protected by a mysterious power.
        //SALVAGE_DEADBODY    -526	It cannot be stolen from party members.
        //SALVAGE_DEADBODY    -519	The soul is about to revive...
        //SALVAGE_DEADBODY    -507	No more corpses can be recovered.

        /// </summary>
    }
}
